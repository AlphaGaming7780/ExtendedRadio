import { AssetCategory, Entity, tutorial } from "cs2/bindings"
import { getModule } from "cs2/modding"
import { FOCUS_DISABLED, Tooltip } from "cs2/ui"
import { AssetDetailPanelContext } from "../asset-detail-panel/asset-detail-panel-context"
import { Loc } from "../../../../common/localization/loc.generated"
import { ReactNode, useContext } from "react"
import { CategoryItemSCSS } from "./category-item.module.scss"
import { TutorialTarget } from "../../tutorials/tutorial-target/tutorial-target"
import classNames from "classnames";
import { AssetCategoryTabBarSCSS } from "./asset-category-tab-bar.module.scss"
import { LockedBadge } from "../badges/locked-badge"
import { IconButton } from "../../../../common/input/button/icon-button"
import { UnlockHighlightBadge } from "../badges/unlock-highlight-badge"
import engine from "cohtml/cohtml"
import { FOCUS_DISABLED$ } from "../../../../common/focus/focus-key"

const path$ = "game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx"

export interface PropsAssetCategoryTabBar {
	categories: Array<AssetCategory>
	selectedCategory: Entity
	onChange: (value: Event) => void,
	onClose: (value: Event) => void,
}

export function AssetCategoryTabBar(propsAssetCategoryTabBar: PropsAssetCategoryTabBar): JSX.Element {
	return getModule(path$, "AssetCategoryTabBar")(propsAssetCategoryTabBar)
}

export const CategoryItem = (subCategory: AssetCategory, selected: Boolean, singleTab: boolean = !1, onSelect: (value: Entity) => void) => {
    
    tutorial.activateTutorialTag(subCategory.uiTag, selected && !subCategory.locked); // sus
    selected && !subCategory.locked && tutorial.triggerTutorialTag(subCategory.uiTag); // sus

    var a = useContext(AssetDetailPanelContext);

    var onClick = () => { onSelect(subCategory.entity) }
    var onMouseOver = () => { a && a.showDetails(subCategory.entity) }
    var onMouseLeave = () => { a && a.hideDetails(subCategory.entity) }

    var child = <>
        {singleTab ?
            <div className={CategoryItemSCSS.singleTab} onMouseOver={onMouseOver} onMouseLeave={onMouseLeave} >
                {/*<TutorialTarget uiTag={subCategory.uiTag} active={!subCategory.locked} disableBlinking={singleTab} children={<div className={CategoryItemSCSS.itemInner}> </div>} />*/}
                <img src={subCategory.icon} className={classNames(AssetCategoryTabBarSCSS.tabIcon, subCategory.locked && AssetCategoryTabBarSCSS.locked)} />
                {subCategory.locked && <LockedBadge {...{ className: AssetCategoryTabBarSCSS.lock }} />}
            </div> :
            <IconButton
                disableHint={true}
                focusKey={FOCUS_DISABLED$}
                src={subCategory.icon}
                selected={selected}
                theme={CategoryItemSCSS}
                className={classNames(subCategory.locked && CategoryItemSCSS.locked)}
                onSelect={onClick}
                onMouseOver={onMouseOver}
                onMouseLeave={onMouseLeave}
            >
                {/*<TutorialTarget uiTag={subCategory.uiTag} active={!subCategory.locked} disableBlinking={singleTab} children={ <div className={CategoryItemSCSS.itemInner}> </div> } />*/}

                {subCategory.highlight && UnlockHighlightBadge({ className: CategoryItemSCSS.highlight })}
                {subCategory.locked && LockedBadge({ className: AssetCategoryTabBarSCSS.lock } )}

            </IconButton>
        }
    </> 

    return <Tooltip tooltip={Loc.SubServices.NAME.renderString(engine, { hash: subCategory.name })} >

        {child}

    </Tooltip>
}
