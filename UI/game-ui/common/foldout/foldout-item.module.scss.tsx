import { getModule } from "cs2/modding"

const path$ = "game-ui/common/foldout/foldout-item.module.scss"

export type PropsFoldoutItemSCSS = {
    "foldout-item": string
    foldoutItem: string
    header: string
    gamepad: string
    "header-content": string
    headerContent: string
    "disable-active-state": string
    disableActiveState: string
    "disable-mouse-states": string
    disableMouseStates: string
    icon: string
    toggle: string
    content: string
    enter: string
    exit: string
    "exit-active": string
    exitActive: string
    "enter-active": string
    enterActive: string
}

export const FoldoutItemSCSS: PropsFoldoutItemSCSS = getModule(path$, "classes")