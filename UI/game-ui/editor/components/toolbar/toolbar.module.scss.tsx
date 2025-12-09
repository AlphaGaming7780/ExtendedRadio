import { getModule } from "cs2/modding"

const path$ = "game-ui/editor/components/toolbar/toolbar.module.scss"

export const EditorToolbarSCSS = {
    editorToolbar: getModule(path$, "classes").editorToolbar,
    button: getModule(path$, "classes").button,
}
