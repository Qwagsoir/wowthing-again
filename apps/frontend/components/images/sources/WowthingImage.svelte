<script lang="ts">
    import type {TippyProps} from '@/types'

    import Image from '../Image.svelte'

    export let name: string
    export let size: number
    export let border = 0
    export let cls: string = undefined
    export let tooltip: TippyProps | string = undefined

    const actualSize = size + (border * 2)

    $: {
        if (name === 'currency/0') {
            name = 'item/135464' // Bulging Sack of Gold
        }
    }
</script>

<Image
    src="https://img.wowthing.org/{size}/{name}.webp"
    alt={name}
    size={actualSize}
    {cls}
    {tooltip}
/>
