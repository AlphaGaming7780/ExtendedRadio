import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-menu.module.scss"

export const AssetMenuSCSS = {
	assetPanel: getModule(path$, "classes").assetPanel,
	gamepadActive: getModule(path$, "classes").gamepadActive,
	detailContainer: getModule(path$, "classes").detailContainer,
	detailPanel: getModule(path$, "classes").detailPanel,
}