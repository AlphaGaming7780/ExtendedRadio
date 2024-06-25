import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/asset-detail-panel/asset-detail-panel.module.scss"

export const AssetDetailPanelSCSS = {
	assetDetailPanel: getModule(path$, "classes").assetDetailPanel,
	titleBar: getModule(path$, "classes").titleBar,
	title: getModule(path$, "classes").title,
	constructionCostField: getModule(path$, "classes").constructionCostField,
	notEnoughMoney: getModule(path$, "classes").notEnoughMoney,
	constructionCostIcon: getModule(path$, "classes").constructionCostIcon,
	row: getModule(path$, "classes").row,
	content: getModule(path$, "classes").content,
	previewContainer: getModule(path$, "classes").previewContainer,
	preview: getModule(path$, "classes").preview,
	column: getModule(path$, "classes").column,
	description: getModule(path$, "classes").description,
	effects: getModule(path$, "classes").effects,
	statsRow: getModule(path$, "classes").statsRow,
	requirementsRow: getModule(path$, "classes").requirementsRow,
	alreadyBuiltRow: getModule(path$, "classes").alreadyBuiltRow,
}