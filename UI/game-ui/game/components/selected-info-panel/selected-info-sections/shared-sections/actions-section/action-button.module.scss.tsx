import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/selected-info-panel/selected-info-sections/shared-sections/actions-section/action-button.module.scss"

export const ActionButtonSCSS = {
	button: getModule(path$, "classes").button,
	icon: getModule(path$, "classes").icon,
}

