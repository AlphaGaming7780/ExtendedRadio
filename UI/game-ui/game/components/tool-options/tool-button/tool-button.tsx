import { FocusKey } from "cs2/bindings"
import { ButtonSounds } from "cs2/input"
import { getModule } from "cs2/modding"
import { TooltipProps } from "cs2/ui"

const path$ = "game-ui/game/components/tool-options/tool-button/tool-button.tsx"

export type PropsToolButton = {
	focusKey?: FocusKey
	src?: string
	selected?: boolean
	multiSelect?: boolean
	disabled?: boolean
	tooltip?: any
	selectSound?: string
	uiTag?: string
	className?: string
	children?: JSX.Element
	onSelect?: (value: Event) => void,
}

export function ToolButton(propsToolButton: PropsToolButton): JSX.Element {
	return getModule(path$, "ToolButton")(propsToolButton)
}
