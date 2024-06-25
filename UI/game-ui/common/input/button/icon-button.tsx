import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input/button/icon-button.tsx"

export type PropsIconButton = {
    src: string,
    tinted?: string,
    theme?: any,
    className?: string,
    children?: JSX.Element,
    [key: string]: any;
}

const IconButtonModule = getModule(path$, "IconButton");

export function IconButton(propsIconButton: PropsIconButton): JSX.Element {
    return <IconButtonModule {...propsIconButton} />
}