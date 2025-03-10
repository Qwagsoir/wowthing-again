﻿namespace Wowthing.Backend.Data
{
    public static partial class Hardcoded
    {
        public static readonly HashSet<int> IgnoredCurrencies = new()
        {
            // Shadowlands
            2010, // [DNT] Byron Test Currency
            
            // Dungeon and Raid
            221, // Emblem of Conquest
            341, // Emblem of Frost
            101, // Emblem of Heroism
            301, // Emblem of Triumph
            102, // Emblem of Valor
            
            // Miscellaneous
            1401, // Stronghold Supplies
            
            // Player vs Player
            121, // Alterac Valley Mark of Honor
            122, // Arathi Basin Mark of Honor 
            123, // Eye of the Storm Mark of Honor
            321, // Isle of Conquest Mark of Honor
            124, // Strand of the Ancients Mark of Honor
            125, // Warsong Gulch Mark of Honor
            126, // Wintergrasp Mark of Honor
            
            // Battle for Azeroth
            1715, // Progenitor Shard TEST
            
            // Warlords of Draenor
            910, // Secret of Draenor Alchemy
            1020, // Secret of Draenor Blacksmithing
            1008, // Secret of Draenor Jewelcrafting
            1017, // Secret of Draenor Leatherworking
            999, // Secret of Draenor Tailoring
            
            // Mists of Pandaria
            810, // Black Iron Fragment
            698, // Zen Jewelcrafter's Token
        };
    }
}
