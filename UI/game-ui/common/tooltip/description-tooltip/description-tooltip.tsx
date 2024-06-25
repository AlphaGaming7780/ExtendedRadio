import { getModule } from "cs2/modding"

const path$ = "game-ui/common/tooltip/description-tooltip/description-tooltip.tsx"

export type PropsDescriptionTooltip = {
    title: string | null,
    description: string | null,
    content: any,
    children: JSX.Element,
}

export type PropsTooltipLayout = {
    title: string | null,
    description: string | null,
    content?: any,
    className?: string
}

export function DescriptionTooltip(propsDescriptionTooltip: PropsDescriptionTooltip): JSX.Element {
    return getModule(path$, "DescriptionTooltip")(propsDescriptionTooltip)
}

export function TooltipLayout(propsTooltipLayout: PropsTooltipLayout): JSX.Element {
    return getModule(path$, "TooltipLayout")(propsTooltipLayout)
}