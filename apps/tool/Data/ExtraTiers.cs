﻿using Wowthing.Tool.Models.Journal;

namespace Wowthing.Tool.Data;

public static partial class Hardcoded
{
    public static readonly List<(DumpJournalTier, List<DumpJournalInstance>)> ExtraTiers = new()
    {
        (
            new DumpJournalTier
            {
                ID = 1,
                Name = "Miscellaneous",
            },
            new()
            {
                new DumpJournalInstance
                {
                    ID = 1,
                    MapID = 2236,
                    Name = "Darkmaul Citadel",
                },
                new DumpJournalInstance
                {
                    ID = 2,
                    Name = "Anniversary World Bosses",
                },
                new DumpJournalInstance
//                {
//                    ID = 3,
//                    Name = "Diablo 4 Launch Event",
//                },
                new DumpJournalInstance
                {
                    ID = 10001,
                    Name = "Old Scholomance",
                },
            }
        )
    };
}
