<script lang="ts">
    import { data as settingsData } from '@/stores/settings'
    import { getWowheadDomain } from '@/utils/get-wowhead-domain'

    export let id: number
    export let noTooltip = false
    export let toComments = false
    export let rename = false
    export let type: string

    let url = ''
    $: {
        url = `https://${getWowheadDomain($settingsData.general.language)}.wowhead.com/${type}=${id}`
        if (toComments) {
            url += '#comments'
        }
    }
</script>

{#if id !== undefined}
    <a
        href="{url}"
        data-disable-wowhead-tooltip="{noTooltip ? 'true' : undefined}"
        data-wh-rename-link="{rename ? 'true' : undefined}"
    >
        <slot />
    </a>
{:else}
    <slot />
{/if}
