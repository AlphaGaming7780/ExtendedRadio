import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.module.scss"

export const StationsMenuSCSS = {
	stationsMenu: getModule(path$, "classes").stationsMenu,
	networks: getModule(path$, "classes").networks,
	stations: getModule(path$, "classes").stations,
	networkItem: getModule(path$, "classes").networkItem,
	stationItem: getModule(path$, "classes").stationItem,
	icon: getModule(path$, "classes").icon,
	column: getModule(path$, "classes").column,
	title: getModule(path$, "classes").title,
	program: getModule(path$, "classes").program,
	progress: getModule(path$, "classes").progress,
}

