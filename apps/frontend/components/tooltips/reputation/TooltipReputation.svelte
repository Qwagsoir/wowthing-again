<script lang="ts">
    import { Constants } from '@/data/constants'
    import { staticStore } from '@/stores/static'
    import { toNiceNumber } from '@/utils/to-nice'
    import type { Character, CharacterReputationParagon } from '@/types'
    import type { StaticDataReputation, StaticDataReputationSet, StaticDataReputationTier } from '@/types/data/static'

    import WowthingImage from '@/components/images/sources/WowthingImage.svelte'

    export let character: Character
    export let characterRep: number
    export let dataRep: StaticDataReputation
    export let paragon: CharacterReputationParagon
    export let reputation: StaticDataReputationSet

    let reps: {
        cls: string
        maxValue: number
        minValue: number
        name: string
        thisOne: boolean
    }[]

    $: {
        reps = []
        const tiers: StaticDataReputationTier = $staticStore.data.reputationTiers[dataRep.tierId] || $staticStore.data.reputationTiers[0]

        for (let i = 0; i < tiers.names.length; i++) {
            const thisOne = (characterRep >= tiers.minValues[i] && characterRep < tiers.maxValues[i])
            reps.push({
                cls: 'quality0',
                maxValue: tiers.maxValues[i],
                minValue: tiers.minValues[i],
                name: tiers.names[i],
                thisOne,
            })
        }

        // Apply quality colours to the bottom 5 tiers
        const start = Math.max(0, reps.length - 1)
        for (let i = start; i >= Math.max(0, start - 5); i--) {
            reps[i].cls = `reputation${start - i + 1}`
            if (reps[i].thisOne && reps[i + 1]) {
                reps[i + 1].thisOne = false
            }
        }

        if (paragon) {
            reps.push({
                cls: 'quality6',
                maxValue: 10000,
                minValue: 0,
                name: 'Paragon',
                thisOne: false,
            })
        }
    }
</script>

<style lang="scss">
    .name {
        padding-left: 0.2rem;
        padding-right: 0.8rem;
        text-align: left;
    }
    .number1 {
        padding-left: 0.8rem;
        text-align: right;
    }
    .number2 {
        padding-left: 0.2rem;
        text-align: right;
    }
    .separator {
        padding: 0;
    }
</style>

<div class="wowthing-tooltip">
    <h4>{character.name}</h4>
    <h5>
        {#if reputation.both === undefined}
            <WowthingImage
                name={character.faction === 0 ? Constants.icons.alliance : Constants.icons.horde}
                size={20}
            />
        {/if}

        {dataRep.name}
    </h5>

    <table class="table-striped">
        <tbody>
            {#each reps as rep}
                <tr class="{rep.cls}">
                    {#if rep.name === 'Paragon'}
                        <td class="drop-shadow number1">{toNiceNumber(paragon.current)}</td>
                        <td class="separator">/</td>
                        <td class="drop-shadow number2">{toNiceNumber(paragon.max)}</td>
                    {:else if characterRep >= rep.maxValue}
                        <td class="number1" colspan="3">✔</td>
                    {:else}
                        <td class="drop-shadow number1">
                            {#if rep.thisOne}
                                {toNiceNumber(characterRep - rep.minValue)}
                            {:else}
                                0
                            {/if}
                        </td>
                        <td class="separator">/</td>
                        <td class="drop-shadow number2">{toNiceNumber(rep.maxValue - rep.minValue)}</td>
                    {/if}
                    <td class="name">{rep.name}</td>
                </tr>
            {/each}

            {#if paragon}
                <tr>
                    <td class="number1" colspan="4">
                        <em>{paragon.received}</em> Paragon reward{paragon.received === 1 ? '' : 's'} received
                    </td>
                </tr>
            {/if}
        </tbody>
    </table>
</div>
