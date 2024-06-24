import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/statistics-panel/menu/item/statistics-item.module.scss"

export const StatisticsItemSCSS = {
	locked: getModule(path$, "classes").locked,
	label: getModule(path$, "classes").label,
}

