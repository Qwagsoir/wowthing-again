<script lang="ts">
    import { achievementStore, userAchievementStore } from '@/stores'
    import { AchievementDataAccount, getAccountData } from '@/utils/achievements'
    import type {AchievementDataAchievement, AchievementDataCriteriaTree} from '@/types'

    import AchievementCriteriaTree from './AchievementsAchievementCriteriaTree.svelte'
    import ProgressBar from '@/components/common/ProgressBar.svelte'

    export let achievement: AchievementDataAchievement

    let criteriaTree: AchievementDataCriteriaTree
    let data: AchievementDataAccount
    let progressBar: boolean
    $: {
        criteriaTree = $achievementStore.data.criteriaTree[achievement.criteriaTreeId]
        data = getAccountData(
            $achievementStore.data,
            $userAchievementStore.data,
            achievement
        )

        progressBar = achievement?.isProgressBar || data.criteria[0]?.isProgressBar || false

        if (achievement.id === 12866) {
            console.log('-- ACCOUNT --')
            console.log(achievement)
            console.log(criteriaTree)
            console.log(data)
        }
    }
</script>

<style lang="scss">
    div {
        display: grid;
        grid-template-columns: 1fr 1fr;
        grid-area: criteria;
        margin-top: 0.5rem;
        padding-top: 0.25rem;
        width: 100%;
    }
</style>

{#if criteriaTree}
    <div>
        {#if progressBar}
            <ProgressBar
                title="{data.criteria[0].description}"
                have={data.have[data.criteria[0].id]}
                total={data.criteria[0].amount}
            />
        {:else}
            {#each criteriaTree.children as child}
                <AchievementCriteriaTree
                    {achievement}
                    criteriaTreeId={child}
                    accountWide={true}
                    haveMap={data.have}
                />
            {/each}
        {/if}
    </div>
{/if}
