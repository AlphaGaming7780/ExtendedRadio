import { AssetCategory, Entity } from "cs2/bindings"
import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-detail-panel/asset-detail-panel.tsx"

export type PropsAssetDetailPanel = {
	entity: Entity,
	className: string,
	upgrade: any,
}

export function AssetDetailPanel(propsAssetDetailPanel: PropsAssetDetailPanel): JSX.Element {
	return getModule(path$, "AssetDetailPanel")(propsAssetDetailPanel)
}
//export function ConstructionCost(propsAssetCategoryTabBar: PropsAssetCategoryTabBar): JSX.Element {
//	return getModule(path$, "AssetCategoryTabBar")(propsAssetCategoryTabBar)
//}
//export function NetConstructionCost(propsAssetCategoryTabBar: PropsAssetCategoryTabBar): JSX.Element {
//	return getModule(path$, "AssetCategoryTabBar")(propsAssetCategoryTabBar)
//}