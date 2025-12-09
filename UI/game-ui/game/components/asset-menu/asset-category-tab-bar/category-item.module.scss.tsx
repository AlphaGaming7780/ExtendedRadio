import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-category-tab-bar/category-item.module.scss"

export const CategoryItemSCSS = {
	button: getModule(path$, "classes").button,
	icon: getModule(path$, "classes").icon,
	locked: getModule(path$, "classes").locked,
	itemInner: getModule(path$, "classes").itemInner,
	highlight: getModule(path$, "classes").highlight,
	lock: getModule(path$, "classes").lock,
	singleTab: getModule(path$, "classes").singleTab
}

