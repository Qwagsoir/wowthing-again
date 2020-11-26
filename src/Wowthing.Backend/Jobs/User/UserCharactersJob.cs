﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wowthing.Backend.Models.API;
using Wowthing.Backend.Models.API.Profile;
using Wowthing.Lib.Enums;
using Wowthing.Lib.Models;
using Wowthing.Lib.Utilities;

namespace Wowthing.Backend.Jobs
{
    public class UserCharactersJob : JobBase
    {
        private const string API_PATH = "profile/user/wow?access_token={0}";

        public override async Task Run(params string[] data)
        {
            var userId = long.Parse(data[0]);
            var accessToken = await _context.UserTokens.FirstOrDefaultAsync(t => t.UserId == userId && t.LoginProvider == "BattleNet" && t.Name == "access_token");
            if (accessToken == null)
            {
                _logger.Error("No access_token for user {0}", userId);
                return;
            }

            var path = string.Format(API_PATH, accessToken.Value);

            // Fetch existing accounts
            var accountMap = await _context.PlayerAccount.Where(a => a.UserId == userId).ToDictionaryAsync(k => (k.Region, k.AccountId));

            // Add any new accounts
            var apiAccounts = new List<(WowRegion, ApiAccountProfileAccount)>();
            foreach (var region in EnumUtilities.GetValues<WowRegion>())
            {
                var uri = GenerateUri(region, ApiNamespace.Profile, path);
                try
                {
                    var profile = await GetJson<ApiAccountProfile>(uri, useAuthorization: false);
                    if (profile?.Accounts == null)
                    {
                        continue;
                    }

                    foreach (ApiAccountProfileAccount account in profile.Accounts)
                    {
                        apiAccounts.Add((region, account));

                        // TODO handle account changing owner? is that even possible?
                        if (!accountMap.ContainsKey((region, account.Id)))
                        {
                            var newAccount = new PlayerAccount
                            {
                                AccountId = account.Id,
                                Region = region,
                                UserId = userId,
                            };
                            _context.PlayerAccount.Add(newAccount);
                            accountMap[(region, account.Id)] = newAccount;
                            _logger.Debug("{0} new account {1}", region, account.Id);
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    _logger.Debug("exception: {e}", e.Message);
                    if (e.Message != "404")
                    {
                        throw e;
                    }
                }
            }

            await _context.SaveChangesAsync();

            // Fetch existing users
            var characterIds = apiAccounts.SelectMany(a => a.Item2.Characters).Select(c => c.Id);
            var characterMap = await _context.PlayerCharacter.Where(c => characterIds.Contains(c.Id)).ToDictionaryAsync(k => k.Id);

            var seenCharacters = new HashSet<long>();
            foreach ((var region, var apiAccount) in apiAccounts)
            {
                var accountId = accountMap[(region, apiAccount.Id)].Id;
                foreach (ApiAccountProfileCharacter apiCharacter in apiAccount.Characters)
                {
                    seenCharacters.Add(apiCharacter.Id);

                    if (!characterMap.TryGetValue(apiCharacter.Id, out PlayerCharacter character))
                    {
                        character = new PlayerCharacter
                        {
                            Id = apiCharacter.Id,
                        };
                        _context.PlayerCharacter.Add(character);
                    }

                    character.AccountId = accountId;
                    character.ClassId = apiCharacter.Class.Id;
                    character.Level = apiCharacter.Level;
                    character.RaceId = apiCharacter.Race.Id;
                    character.RealmId = apiCharacter.Realm.Id;
                    character.Faction = apiCharacter.Faction.EnumParse<WowFaction>();
                    character.Gender = apiCharacter.Gender.EnumParse<WowGender>();
                    character.Name = apiCharacter.Name;
                }
            }

            await _context.SaveChangesAsync();

            // Orphan characters not seen
            //await _characterRepository.OrphanCharacters(apiAccounts.Select(a => a.Id), seenCharacters);
        }
    }
}
