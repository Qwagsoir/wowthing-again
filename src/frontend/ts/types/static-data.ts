import type {Dictionary} from './dictionary'


export class StaticData {
    Classes: Dictionary<StaticDataClass>
    Races: Dictionary<StaticDataRace>
    Realms: Dictionary<StaticDataRealm>
    Reputations: Dictionary<StaticDataReputation>
    ReputationTiers: Dictionary<StaticDataReputationTier>

    MountSets: any // FIXME
    SpellToMount: Dictionary<number>

    PetSets: any // FIXME
    CreatureToPet: Dictionary<number>

    ReputationSets: StaticDataSetCategory<StaticDataReputationSet>[]
}

class StaticDataClass {
    Id: number
    Name: string
    Icon: string
    SpecializationIds: number[]
}

class StaticDataRace {
    Id: number
    Name: string
    IconFemale: string
    IconMale: string
}

class StaticDataRealm {
    Id: number
    Region: number
    Name: string
    Slug: string
}

export class StaticDataReputation {
    Id: number
    Name: string
    TierId: number
}

export class StaticDataReputationTier {
    Id: number
    MinValues: number[]
    MaxValues: number[]
    Names: string[]
}

class StaticDataSetCategory<T> {
    Name: string
    Reputations: T[][]
    Slug: string
}

export class StaticDataReputationSet {
    Both: StaticDataReputationReputation
    Alliance: StaticDataReputationReputation
    Horde: StaticDataReputationReputation
    Paragon: boolean

    get tooltip(): string {
        if (this.Both) {
            return this.Both.Name;
        }
        else {
            return `[A] ${this.Alliance.Name}<br>[H] ${this.Horde.Name}`
        }
    }
}

class StaticDataReputationReputation {
    Id: number
    Name: string
    Icon: string
    Note: string
}
