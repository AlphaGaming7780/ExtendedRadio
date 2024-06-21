//import { ExtraLibUI } from "../../../../ExtraLibUI"
import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input/text/int-input.tsx"

export type PropsIntInput = {
	min?: number,
	max?: number
}

export function IntInput(propsIntInput: PropsIntInput) : JSX.Element
{
	return getModule(path$, "IntInput")(propsIntInput)
}