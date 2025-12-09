import { Theme } from "cs2/bindings";
import { getModule } from "cs2/modding"

const path$ = "game-ui/common/input/drag-handle.tsx"

export type PropsDragHandle = {
    onDrag: (mouseX: number, mouseY: number, panelWidth: number, panelHeight: number) => void,
    children: any,
}

const DragHandleModule = getModule(path$, "DragHandle");

export function DragHandle(propsDragHandle: PropsDragHandle): JSX.Element {
    return <DragHandleModule {...propsDragHandle} />
}