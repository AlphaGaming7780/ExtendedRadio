import { FocusKey } from "cs2/ui"
import { ReactNode } from "react"
import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input/slider/slider.tsx"

export type PropsSlider = {
	focusKey?: FocusKey, 
	debugName?: string, 
	value: number, 
	start: number, 
	end: number, 
	gamepadStep: number, 
	disabled: boolean, 
	vertical?: boolean, 
	sounds?: SliderSounds, 
	thumb?: ReactNode, 
	theme?: any, 
	className?: string, 
	style?: string, 
	children?: string, 
	noFill: boolean, 
	valueTransformer?: SliderValueTransformer, 
	onChange?: (value: number) => void, 
	onDragStart?: () => void, 
	onDragEnd?: () => void, 
	onMouseOver?: () => void, 
	onMouseLeave?: () => void
}
export type SliderSounds = {dragStart: string, drag: string, scaleDragVolume: boolean}
export enum SliderValueTransformer { floatTransformer, intTransformer = getModule(path$, "intTransformer"), useStepTransformer = getModule(path$, "useStepTransformer") }

export function Slider(propsSlider: PropsSlider) : JSX.Element
{
	if(propsSlider.valueTransformer == SliderValueTransformer.floatTransformer) propsSlider.valueTransformer = undefined

	return getModule(path$, "Slider").render(propsSlider)

}