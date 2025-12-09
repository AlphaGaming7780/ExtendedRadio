import { getModule } from "cs2/modding"

const path$ = "game-ui/editor/themes/editor-tool-button.module.scss"

export const EditorToolButtonSCSS = {
    button: getModule(path$, "classes").button,
    icon: getModule(path$, "classes").icon,
    hint: getModule(path$, "classes").hint,
}