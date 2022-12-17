import toPairs from 'lodash/toPairs'
import { get } from 'svelte/store'

import { userModifiedStore } from './user-modified'
import { WritableFancyStore } from '@/types'
import type { UserQuestData } from '@/types/data'


export class UserQuestDataStore extends WritableFancyStore<UserQuestData> {
    get dataUrl(): string {
        let url = document.getElementById('app')?.getAttribute('data-user')
        if (url) {
            const modified = get(userModifiedStore).data.quests
            url = url.replace(/\/(?:public|private).+$/, `/quests-${modified}.json`)
        }
        return url
    }

    initialize(userQuestData: UserQuestData): void {
        console.time('UserQuestDataStore.initialize')

        for (const [, characterData] of toPairs(userQuestData.characters)) {
            if (characterData.dailyQuests === undefined) {
                characterData.dailyQuests = new Map<number, boolean>()
                for (const questId of characterData.dailyQuestList) {
                    characterData.dailyQuests.set(questId, true)
                }
                characterData.dailyQuestList = null
            }

            if (characterData.quests === undefined) {
                characterData.quests = new Map<number, boolean>()
                for (const questId of characterData.questList) {
                    characterData.quests.set(questId, true)
                }
                characterData.questList = null
            }
        }

        console.timeEnd('UserQuestDataStore.initialize')
    }
}

export const userQuestStore = new UserQuestDataStore()
