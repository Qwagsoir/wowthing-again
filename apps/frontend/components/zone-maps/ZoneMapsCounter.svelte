<script lang="ts">
    import { manualStore } from '@/stores'
    import getPercentClass from '@/utils/get-percent-class'
    import type { UserCount } from '@/types'
    import type { RewardType } from '@/types/enums'

    export let key: string
    export let type: RewardType

    let counts: UserCount
    $: {
        counts = $manualStore.data.zoneMaps.typeCounts[key][type]
    }
</script>

<style lang="scss">
    div {
        background: $highlight-background;
        border: 1px solid $border-color;
        border-radius: 0 0 $border-radius $border-radius;
        border-top-width: 0;
        //font-size: 90%;
        line-height: 1;
        margin: 2.5px -3px 0 2.5px;
        padding: 0 0.3rem 0.2rem 0.3rem;
        word-spacing: -0.2ch;
        z-index: 10;
    }
</style>

{#if counts && counts.total > 0}
    <div class="{getPercentClass(counts.have / counts.total * 100)}">{counts.have} / {counts.total}</div>
{/if}
