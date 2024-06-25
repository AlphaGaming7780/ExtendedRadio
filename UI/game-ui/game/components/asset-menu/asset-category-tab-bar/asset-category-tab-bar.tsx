import { AssetCategory, Entity } from "cs2/bindings"
import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx"

export type PropsAssetCategoryTabBar = {
	categories: Array<AssetCategory>
	selectedCategory: Entity
	onChange: (value: Event) => void,
	onClose: (value: Event) => void,
}

export function AssetCategoryTabBar(propsAssetCategoryTabBar: PropsAssetCategoryTabBar): JSX.Element {
	return getModule(path$, "AssetCategoryTabBar")(propsAssetCategoryTabBar)
}
