import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/statistics-panel/menu/item/statistics-checkbox.module.scss"

export const StatisticsCheckboxSCSS = {
	toggle: getModule(path$, "classes").toggle,
}

