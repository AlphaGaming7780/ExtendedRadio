import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.module.scss"

export const AssetCategoryTabBarSCSS = {
	assetCategoryTabBar: getModule(path$, "classes").assetCategoryTabBar,
	tabIcon: getModule(path$, "classes").tabIcon,
	locked: getModule(path$, "classes").locked,
	lock: getModule(path$, "classes").lock,
	items: getModule(path$, "classes").items,
	closeButton: getModule(path$, "classes").closeButton,
}

