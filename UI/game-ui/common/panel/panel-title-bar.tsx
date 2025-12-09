import { Theme } from "cs2/bindings";
import { getModule } from "cs2/modding"

const path$ = "game-ui/common/panel/panel-title-bar.tsx"

export type PropsPanelTitleBar = {
    icon: string,
    theme?: any,
    onCloseOverride?: () => void,
    className?: string,
    children?: any,
    [key: string]: any;
}

const PanelTitleBarModule = getModule(path$, "PanelTitleBar");

export function PanelTitleBar(propsPanelTitleBar: PropsPanelTitleBar): JSX.Element {
    return <PanelTitleBarModule {...propsPanelTitleBar} />
}