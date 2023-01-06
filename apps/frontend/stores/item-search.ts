import sortBy from 'lodash/sortBy'
import { get, writable } from 'svelte/store'

import { userStore } from '@/stores'
import { ItemLocation } from '@/enums'
import type {
    ItemSearchResponseCharacter,
    ItemSearchResponseGuildBank,
    ItemSearchResponseItem
} from '@/types/items'


export class ItemSearchState {
    public searchTerms = ''
    public location = ItemLocation.Any

    private static minimumTermsLength = 3
    private static url = '/api/item-search'

    async search(): Promise<ItemSearchResponseItem[]> {
        const xsrf = document.getElementById('app')
            .getAttribute('data-xsrf')

        const data = {
            terms: this.searchTerms,
            location: this.location,
        }

        const response = await fetch(ItemSearchState.url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': xsrf,
            },
            body: JSON.stringify(data),
        })

        if (response.ok) {
            const result = await response.json() as ItemSearchResponseItem[]
            const userData = get(userStore)

            for (const item of result) {
                // Combine character items into a single stack
                const characterMap: Record<string, ItemSearchResponseCharacter[]> = {}
                for (const character of (item.characters || [])) {
                    const key = [
                        character.characterId,
                        character.location,
                        character.quality,
                        character.itemLevel,
                        (character.bonusIds || []).join(':'),
                    ].join('|')

                    characterMap[key] ||= []
                    characterMap[key].push(character)
                }

                const newCharacters: ItemSearchResponseCharacter[] = []
                for (const key of Object.keys(characterMap)) {
                    const character = characterMap[key][0]
                    character.count = characterMap[key].reduce((a: number, b) => a + b.count, 0)
                    newCharacters.push(character)
                }

                item.characters = sortBy(
                    newCharacters,
                    (char) => [
                        userData.characterMap[char.characterId].realm.region,
                        userData.characterMap[char.characterId].realm.name,
                    ]
                )

                // Combine guild items into a single stack
                const guildBankMap: Record<string, ItemSearchResponseGuildBank[]> = {}
                for (const guildBank of (item.guildBanks || [])) {
                    const key = [
                        guildBank.guildId,
                        guildBank.tab,
                        guildBank.quality,
                        guildBank.itemLevel,
                        (guildBank.bonusIds || []).join(':'),
                    ].join('|')

                    guildBankMap[key] ||= []
                    guildBankMap[key].push(guildBank)
                }

                const newGuildBanks: ItemSearchResponseGuildBank[] = []
                for (const key of Object.keys(guildBankMap)) {
                    const guildBank = guildBankMap[key][0]
                    guildBank.count = guildBankMap[key].reduce((a: number, b) => a + b.count, 0)
                    newGuildBanks.push(guildBank)
                }

                item.guildBanks = sortBy(
                    newGuildBanks,
                    (guild) => [
                        userData.guilds[guild.guildId].realm.region,
                        userData.guilds[guild.guildId].realm.name,
                        userData.guilds[guild.guildId].name,
                        guild.tab,
                        guild.slot,
                    ]
                )
            }

            return result
        }
    }

    get isValid(): boolean {
        return this.searchTerms.trim().length >= ItemSearchState.minimumTermsLength
    }
}


export const itemSearchState = writable<ItemSearchState>(new ItemSearchState())
