export const categoryOrder: number[] = [
    245, // Shadowlands
    143, // Battle for Azeroth
    141, // Legion
    137, // Warlords of Draenor
    133, // Mists of Pandaria
    81, // Cataclysm
    21, // Wrath of the Lich King
    23, // Burning Crusade
    //4, // Classic
    0,
    22, // Dungeon and Raid
    1, // Miscellaneous
    2, // Player vs. Player
]

export const currencyItems: Record<number, number[]> = {
    // Shadowlands
    245: [
        188957, // Genesis Mote
        190189, // Sandworn Relic
    ],
    // Battle for Azeroth
    143: [
        152668, // Expulsom
        162460, // Hydrocore
        168802, // Nazjatar Battle Commendation
    ],
    // Legion
    141: [
        124124, // Blood of Sargeras
        146963, // Desecrated Seaweed
        153021, // Intact Demon Eye
    ],
    // Warlords of Draenor
    137: [
        124099, // Blackfang Claw
    ],
    // Burning Crusade
    23: [
        26045, // Halaa Battle Token
        26044, // Halaa Research Token
    ],
    // Player vs Player
    2: [
        137642, // Mark of Honor
    ],
    // Miscellaneous
    1: [
        163036, // Polished Pet Charm
        116415, // Shiny Pet Charm
        37829, // Brewfest Prize Token
        23247, // Burning Blossom
        21100, // Coin of Ancestry
        49927, // Love Token
        44791, // Noblegarden Chocolate
        33226, // Tricky Treat
    ],
}

const skipCurrencies: number[] = [
    // Shadowlands
    1743, // Fake Anima for Quest Tracking
    1754, // Argent Commendation??
    1802, // Shadowlands PvP Weekly Reward Progress
    1811, // zzoldSanctum Architect
    1812, // zzoldSanctum Anima Weaver
    1829, // Renown-Kyrian
    1830, // Renown-Venthyr
    1831, // Renown-Night Fae
    1832, // Renown-Necrolord
    1859, // Reservoir Anima-Kyrian
    1860, // Reservoir Anima-Venthyr
    1861, // Reservoir Anima-Night Fae
    1862, // Reservoir Anima-Necrolord
    1863, // Redeemed Soul-Kyrian
    1864, // Redeemed Soul-Venthyr
    1865, // Redeemed Soul-Night Fae
    1866, // Redeemed Soul-Necrolord
    1867, // Sanctum Architect-Kyrian
    1868, // Sanctum Architect-Venthyr
    1869, // Sanctum Architect-Night Fae
    1870, // Sanctum Architect-Necrolord
    1871, // Sanctum Anima Weaver-Kyrian
    1872, // Sanctum Anima Weaver-Venthyr
    1873, // Sanctum Anima Weaver-Night Fae
    1874, // Sanctum Anima Weaver-Necrolord

    // Legion
    1355, // Felessence
    1356, // Echoes of Battle
    1357, // Echoes of Domination

    // Warlords of Draenor
    897, // UNUSED

    // Miscellaneous
    1, // Currency Token Test Token 4
    2, // Currency Token Test Token 2
    4, // Currency Token Test Token 5
    42, // Badge of Justice
    1388, // Armor Scraps
    1835, // Linked Currency Test (Src) - PTH
    1836, // Linked Currency Test (Dst) - PTH

    // Player vs. Player
    103, // Arena Points
    104, // Honor Points DEPRECATED
    161, // Stone Keeper's Shard
    181, // Honor Points DEPRECATED2
    201, // Venture Coin
]

export const skipCurrenciesMap: Record<number, boolean> = Object.fromEntries(
    skipCurrencies.map((currencyId) => [currencyId, true])
)
