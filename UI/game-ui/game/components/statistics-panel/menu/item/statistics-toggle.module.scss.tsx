import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/statistics-panel/menu/item/statistics-toggle.module.scss"

export const StatisticsToggleSCSS = {
	size: getModule(path$, "classes").size,
	toggle: getModule(path$, "classes").toggle,
}

