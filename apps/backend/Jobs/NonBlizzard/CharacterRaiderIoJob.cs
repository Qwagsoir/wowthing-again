﻿using Wowthing.Backend.Models.API.NonBlizzard;
using Wowthing.Lib.Models.Player;
using Wowthing.Lib.Models.Query;

namespace Wowthing.Backend.Jobs.NonBlizzard
{
    public class CharacterRaiderIoJob : JobBase
    {
        private const string ApiUrl = "https://raider.io/api/v1/characters/profile?region={0}&realm={1}&name={2}&fields=mythic_plus_scores_by_season:{3}";

        public override async Task Run(params string[] data)
        {
            var query = JsonConvert.DeserializeObject<SchedulerCharacterQuery>(data[0]) ??
                        throw new InvalidJsonException(data[0]);
            using var shrug = CharacterLog(query);

            // Fetch seasons
            var seasonIds = JsonConvert
                .DeserializeObject<int[]>(data[1])
                .EmptyIfNull();

            var oofParts = new List<string>();
            foreach (var seasonId in seasonIds)
            {
                var rioSeasons = ApiCharacterRaiderIoSeason.SeasonMap
                    .Where(kvp => kvp.Value == seasonId)
                    .ToArray();
                if (rioSeasons.Length > 0)
                {
                    oofParts.Add(rioSeasons[0].Key);
                }
            }

            if (oofParts.Count == 0)
            {
                Logger.Information("No matching seasons found: {Seasons}", string.Join(",", seasonIds.Select(id => id.ToString())));
                return;
            }
            
            var oof = string.Join(":", oofParts);
            
            // Fetch API data
            var uri = new Uri(string.Format(ApiUrl, query.Region.ToString().ToLowerInvariant(), query.RealmSlug, query.CharacterName, oof));

            var result = await GetJson<ApiCharacterRaiderIo>(uri, useAuthorization: false, useLastModified: false);
            /*if (result.NotModified)
            {
                LogNotModified();
                return;
            }*/

            // Fetch character data
            var raiderIo = await Context.PlayerCharacterRaiderIo.FindAsync(query.CharacterId);
            if (raiderIo == null)
            {
                raiderIo = new PlayerCharacterRaiderIo
                {
                    CharacterId = query.CharacterId,
                };
                Context.PlayerCharacterRaiderIo.Add(raiderIo);
            }

            var seasons = new Dictionary<int, PlayerCharacterRaiderIoSeasonScores>(raiderIo.Seasons.EmptyIfNull());
            foreach (var season in result.Data.ScoresBySeason.EmptyIfNull())
            {
                seasons[season.SeasonId] = new PlayerCharacterRaiderIoSeasonScores
                {
                    All = season.ScoreAll,
                    Dps = season.ScoreDps,
                    Healer = season.ScoreHealer,
                    Tank = season.ScoreTank,
                    Spec1 = season.ScoreSpec1,
                    Spec2 = season.ScoreSpec2,
                    Spec3 = season.ScoreSpec3,
                    Spec4 = season.ScoreSpec4,
                };
            }

            raiderIo.Seasons = seasons;

            await Context.SaveChangesAsync();
        }
    }
}
