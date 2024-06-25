import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/selected-info-panel/shared-components/info-row/info-row.module.scss"

export const InfoRowSCSS = {
	infoRow: getModule(path$, "classes").infoRow,
	disableFocusHighlight: getModule(path$, "classes").disableFocusHighlight,
	link: getModule(path$, "classes").link,
	tooltipRow: getModule(path$, "classes").tooltipRow,
	left: getModule(path$, "classes").left,
	hasIcon: getModule(path$, "classes").hasIcon,
	right: getModule(path$, "classes").right,
	icon: getModule(path$, "classes").icon,
	uppercase: getModule(path$, "classes").uppercase,
	subRow: getModule(path$, "classes").subRow,
}

