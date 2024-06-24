import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input/toggle/checkbox/checkbox.tsx"

export type PropsCheckbox = {
    checked?: boolean,
    disabled?: boolean,
    theme?: any,
    className?: string,
    [key: string]: any;
}

const CheckboxModule = getModule(path$, "Checkbox")

export function Checkbox(propsCheckbox: PropsCheckbox): JSX.Element {
    return <CheckboxModule {...propsCheckbox} />
}