import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input/button/icon-button.tsx"

export type PropsIconButton = {
    src: string,
    tinted?: boolean,
    theme?: any,
    className?: string,
    children?: any,
    [key: string]: any;
}


const IconButtonModule = getModule(path$, "IconButton");

export function IconButton(propsIconButton: PropsIconButton): JSX.Element {
    return <IconButtonModule {...propsIconButton} />
}