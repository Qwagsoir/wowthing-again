<script lang="ts">
    import IntersectionObserver from 'svelte-intersection-observer'

    import { userTransmogStore } from '@/stores'
    import { journalState } from '@/stores/local-storage'
    import { data as settingsData } from '@/stores/settings'
    import getPercentClass from '@/utils/get-percent-class'
    import getFilteredItems from '@/utils/journal/get-filtered-items'
    import type { UserCount } from '@/types'
    import type { JournalDataEncounterItem, JournalDataEncounterItemGroup } from '@/types/data'

    import CollectionCount from '@/components/collections/CollectionCount.svelte'
    import Item from './JournalItem.svelte'

    export let bonusIds: Record<number, number>
    export let group: JournalDataEncounterItemGroup
    export let instanceExpansion: number
    export let stats: UserCount

    let element: HTMLElement
    let intersected: boolean
    let items: JournalDataEncounterItem[]
    let percent: number
    $: {
        items = getFilteredItems(
            $journalState,
            $settingsData,
            $userTransmogStore.data,
            instanceExpansion,
            group
        )
        percent = Math.floor((stats?.have ?? 0) / (stats?.total ?? 1) * 100)
    }
</script>

<style lang="scss">
    .collection-objects {
        min-height: 52px;
    }
</style>

{#if items.length > 0}
    <div class="collection-group">
        <h4 class="drop-shadow {getPercentClass(percent)}">
            {group.name}
            <CollectionCount counts={stats} />
        </h4>

        <div
            bind:this={element}
            class="collection-objects"
        >
            <IntersectionObserver
                bind:intersecting={intersected}
                once
                {element}
            >
                {#if intersected}
                    {#each items as item}
                        <Item
                            {bonusIds}
                            {item}
                        />
                    {/each}
                {/if}
            </IntersectionObserver>
        </div>
    </div>
{/if}
