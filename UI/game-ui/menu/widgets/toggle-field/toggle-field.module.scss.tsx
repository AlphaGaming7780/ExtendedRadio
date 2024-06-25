import { getModule } from "cs2/modding"

const path$ = "game-ui/menu/widgets/toggle-field/toggle-field.module.scss"

export const ToggleFieldSCSS = {
	toggle: getModule(path$, "classes").toggle,
	radioToggle: getModule(path$, "classes").radioToggle,
	bullet: getModule(path$, "classes").bullet,
}

