import { Theme } from "cs2/bindings";
import { getModule } from "cs2/modding"
import { PanelProps } from "cs2/ui";

const path$ = "game-ui/common/panel/collapsible-panel.tsx"

export interface PropsCollapsiblePanel extends PanelProps {
    theme?: any,
    onClose?: () => void,
    expanded?: boolean,
    headerText: string | null,
    onToggleExpanded?: () => void,
    className?: string,
    children?: any,
    isFocusRoot?: any,
    headerIcon?: any,
    togglable?: boolean,
}

const CollapsiblePanelModule = getModule(path$, "CollapsiblePanel");

export function CollapsiblePanel(propsCollapsiblePanel: PropsCollapsiblePanel): JSX.Element {
    return <CollapsiblePanelModule {...propsCollapsiblePanel} />
}