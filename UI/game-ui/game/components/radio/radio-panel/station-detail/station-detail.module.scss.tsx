import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/radio/radio-panel/station-detail/station-detail.module.scss"

export const StationDetailSCSS = {
	stationDetail: getModule(path$, "classes").stationDetail,
	header: getModule(path$, "classes").header,
	stationName: getModule(path$, "classes").stationName,
	sectionTitle: getModule(path$, "classes").sectionTitle,
	list: getModule(path$, "classes").list,
	programItem: getModule(path$, "classes").programItem,
	time: getModule(path$, "classes").time,
	column: getModule(path$, "classes").column,
	title: getModule(path$, "classes").title,
	description: getModule(path$, "classes").description,
	progress: getModule(path$, "classes").progress,
}

