import { getModule } from "cs2/modding"
import { ImgHTMLAttributes } from "react"

const path$ = "game-ui/common/foldout/foldout-item.tsx"

export type PropsFoldoutItem = {
    header: any,
    theme?: any,
    type?: string,
    nesting?: any
    initialExpanded: boolean,
    expandFromContent: boolean,
    onSelect?: Function,
    onToggleExpanded?: Function,
    className?: string,
    children?: any,
}

export type PropsFoldoutItemHeader = {
    icon: JSX.Element,
    children: JSX.Element,
}

export function FoldoutItem(propsFoldoutItem: PropsFoldoutItem): JSX.Element {
    return getModule(path$, "FoldoutItem")(propsFoldoutItem)
}

export function FoldoutItemHeader(propsFoldoutItemHeader: PropsFoldoutItemHeader): JSX.Element {
    return getModule(path$, "FoldoutItemHeader")(propsFoldoutItemHeader)
}