import { AssetCategory, Entity } from "cs2/bindings"
import { getModule } from "cs2/modding"
import { Context } from "react";

const path$ = "game-ui/game/components/asset-menu/asset-detail-panel/asset-detail-panel-context.ts"

export const AssetDetailPanelContext: Context<AssetDetailPanelContextf> = getModule(path$, "AssetDetailPanelContext");

export class AssetDetailPanelContextf {
	showDetails(entity: Entity) : void {}
	hideDetails(entity: Entity): void {}
}