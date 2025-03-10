<script lang="ts">
    import filter from 'lodash/filter'
    import find from 'lodash/find'

    import { classOrder } from '@/data/character-class'
    import { iconStrings } from '@/data/icons'
    import { manualStore, staticStore } from '@/stores'
    import { zoneMapState } from '@/stores/local-storage/zone-map'
    import { zoneMapMedia } from '@/stores/media-queries/zone-map'
    import { RewardType, PlayableClass } from '@/types/enums'
    import type { FarmStatus } from '@/types'
    import type { ManualDataZoneMapCategory, ManualDataZoneMapFarm } from '@/types/data/manual'

    import Checkbox from '@/components/forms/CheckboxInput.svelte'
    import ClassIcon from '@/components/images/ClassIcon.svelte'
    import Counter from './ZoneMapsCounter.svelte'
    import IconifyIcon from '@/components/images/IconifyIcon.svelte'
    import Image from '@/components/images/Image.svelte'
    import Thing from './ZoneMapsThing.svelte'

    export let slug1: string
    export let slug2: string

    let categories: ManualDataZoneMapCategory[]
    let farms: ManualDataZoneMapFarm[]
    let farmStatuses: FarmStatus[]
    let height: number
    let width: number
    let slugKey: string

    $: {
        categories = filter(
            find($manualStore.data.zoneMaps.sets, (s) => s !== null && s[0].slug === slug1),
            (s) => s?.farms?.length > 0
        )
        if (slug2) {
            categories = filter(categories, (s) => s.slug === slug2)
        }
        slugKey = slug2 ? `${slug1}--${slug2}` : slug1

        if (categories.length > 0) {
            farms = [...categories[0].farms]
            for (const vendorId of ($manualStore.data.shared.vendorsByMap[categories[0].mapName] || [])) {
                farms.push(...$manualStore.data.shared.vendors[vendorId].asFarms($staticStore.data, categories[0].mapName))
            }

            farmStatuses = $manualStore.data.zoneMaps.farmStatus[slugKey]
        }
        if ($zoneMapState.classFilters[slugKey] === undefined) {
            $zoneMapState.classExpanded[slugKey] = false
            $zoneMapState.classFilters[slugKey] = {}
        }
    }

    $: {
        [width, height] = $zoneMapMedia
    }
</script>

<style lang="scss">
    .farm {
        --image-border-radius: #{$border-radius-large};
        --image-border-width: 2px;

        position: relative;
    }
    .toggles {
        background: $highlight-background;
        border: 1px solid $border-color;
        border-radius: $border-radius;
        display: flex;
        justify-content: center;
        padding: 0.2rem 0.3rem;
        position: absolute;
        white-space: nowrap;
        z-index: 10;
    }
    .setting-toggles {
        left: 50%;
        top: 1px;
        transform: translateX(-50%);

        & :global(fieldset:not(:first-child)) {
            margin-left: 0.3rem;
        }
    }
    .class-toggles {
        --image-border-radius: 50%;

        align-items: center;
        cursor: pointer;
        padding-left: 0.5rem;
        right: 1px;
        top: 1px;

        :global(img) {
            margin-left: 2px;
        }
    }
    .class-list {
        flex-direction: column;
        position: absolute;
        right: 1px;
        top: 2.1rem;
    }
    .toggle-group {
        display: flex;
        height: 1.5rem;

        &:not(:first-child) {
            border-left: 1px solid $border-color;
            margin-left: 0.5rem;
            padding-left: 0.5rem;
        }
    }
    .checkbox-counter {
        align-items: flex-end;
        display: flex;
        flex-direction: column;
    }
    .credits {
        bottom: 1px;
        display: flex;
        flex-direction: row-reverse;
        justify-content: space-between;
        padding: 0 1px;
        position: absolute;
        width: 100%;
        z-index: 10;

        div {
            background: $highlight-background;
            border: 1px solid $border-color;
            border-radius: $border-radius;
            padding: 0.1rem 0.4rem 0.2rem;
        }
    }
</style>

{#if categories.length > 0 && farmStatuses}
    <div class="farm">
        <div class="toggles setting-toggles">
            <div class="toggle-group">
                <Checkbox
                    name="show_completed"
                    bind:value={$zoneMapState.showCompleted}
                >Completed</Checkbox>
                <Checkbox
                    name="show_killed"
                    bind:value={$zoneMapState.showKilled}
                >Killed</Checkbox>
            </div>

            <div class="toggle-group">
                <div class="checkbox-counter">
                    <Checkbox
                        name="track_achievements"
                        bind:value={$zoneMapState.trackAchievements}
                    >Achievements</Checkbox>

                    <Counter key={slugKey} type={RewardType.Achievement} />
                </div>

                <div class="checkbox-counter">
                    <Checkbox
                        name="track_mounts"
                        bind:value={$zoneMapState.trackMounts}
                    >Mounts</Checkbox>

                    <Counter key={slugKey} type={RewardType.Mount} />
                </div>

                <div class="checkbox-counter">
                    <Checkbox
                        name="track_pets"
                        bind:value={$zoneMapState.trackPets}
                    >Pets</Checkbox>

                    <Counter key={slugKey} type={RewardType.Pet} />
                </div>

                <div class="checkbox-counter">
                    <Checkbox
                        name="track_quests"
                        bind:value={$zoneMapState.trackQuests}
                    >Quests</Checkbox>

                    <Counter key={slugKey} type={RewardType.Quest} />
                </div>

                <div class="checkbox-counter">
                    <Checkbox
                        name="track_toys"
                        bind:value={$zoneMapState.trackToys}
                    >Toys</Checkbox>

                    <Counter key={slugKey} type={RewardType.Toy} />
                </div>

                <div class="checkbox-counter">
                    <Checkbox
                        name="track_transmog"
                        bind:value={$zoneMapState.trackTransmog}
                    >Transmog</Checkbox>

                    <Counter key={slugKey} type={RewardType.Transmog} />
                </div>
            </div>
        </div>

        <div
            class="toggles class-toggles"
            on:click={() => $zoneMapState.classExpanded[slugKey] = !$zoneMapState.classExpanded[slugKey]}
        >
            Class:

            {#each classOrder.filter((c) => $zoneMapState.classFilters[slugKey][c] === true) as classId}
                <ClassIcon size={20} classId={classId} />
            {:else}
                ALL
            {/each}

            <IconifyIcon
                icon={iconStrings['chevron-' + ($zoneMapState.classExpanded[slugKey] ? 'down' : 'right')]}
            />
        </div>

        {#if $zoneMapState.classExpanded[slugKey]}
            <div class="toggles class-list">
                {#each classOrder as classId}
                    <Checkbox
                        name="class_{classId}"
                        textClass="class-{classId}"
                        bind:value={$zoneMapState.classFilters[slugKey][classId]}
                    >{PlayableClass[classId]}</Checkbox>
                {/each}
            </div>
        {/if}

        <Image
            src="https://img.wowthing.org/maps/{categories[0].mapName}_{width}_{height}.webp"
            alt="Map of {categories[0].name}"
            border={2}
            {width}
            {height}
        />

        {#each farms as farm, farmIndex}
            <Thing
                status={farmStatuses[farmIndex]}
                {farm}
            />
        {/each}

        <div class="credits">
            <div>
                Data sources:
                <a href="https://github.com/zarillion/handynotes-plugins">HandyNotes Plugins</a> /
                <a href="https://www.wowdb.com">WoWDB</a> /
                <a href="https://www.wowhead.com">Wowhead</a> /
                <a href="https://wow.tools">WoW.tools</a>
            </div>

            {#if categories[0].wowheadGuide}
                <div>
                    <a href="{categories[0].wowheadGuide}">Wowhead guide</a>
                </div>
            {/if}
        </div>
    </div>
{/if}
