import { getModule } from "cs2/modding"

const path$ = "game-ui/common/panel/panel.module.scss"

export const PanelSCSS = {
	closeIcon: getModule(path$, "classes").closeIcon,
	toggleIcon: getModule(path$, "classes").toggleIcon,
	toggleIconExpanded: getModule(path$, "classes").toggleIconExpanded,
	panel: getModule(path$, "classes").panel,
	header: getModule(path$, "classes").header,
	content: getModule(path$, "classes").content,
	footer: getModule(path$, "classes").footer,
	titleBar: getModule(path$, "classes").titleBar,
	title: getModule(path$, "classes").title,
	icon: getModule(path$, "classes").icon,
	iconSpace: getModule(path$, "classes").iconSpace,
	closeButton: getModule(path$, "classes").closeButton,
	toggle: getModule(path$, "classes").toggle
}