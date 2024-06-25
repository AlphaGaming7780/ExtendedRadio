import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input/text/float-input.tsx"

export type PropsFloatInput = {
    min?: number,
    max?: number,
    fractionDigits?: number
}

export function IntInput(propsFloatInput: PropsFloatInput) : JSX.Element
{
    return getModule(path$, "FloatInput")(propsFloatInput)
}
