import { AssetCategory, Entity } from "cs2/bindings"
import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/asset-menu/badges/locked-badge.tsx"

export type PropsLockedBadge = { style?: any, className?: string }

export function LockedBadge(propsLockedBadge: PropsLockedBadge): JSX.Element {
	return getModule(path$, "LockedBadge")(propsLockedBadge)
}