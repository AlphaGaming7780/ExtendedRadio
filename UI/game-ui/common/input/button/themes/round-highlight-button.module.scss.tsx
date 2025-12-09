import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input/button/themes/round-highlight-button.module.scss"

export const RoundHighlightButtonSCSS = {
	button: getModule(path$, "classes").button,
}