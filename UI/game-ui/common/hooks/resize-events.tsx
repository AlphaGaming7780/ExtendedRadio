import { Theme } from "cs2/bindings";
import { getModule } from "cs2/modding"

const path$ = "game-ui/common/hooks/resize-events.tsx"

export type PropsUseElementRect = {
    e: any
    t?: boolean,
}

const UseElementRectModule = getModule(path$, "useElementRect");

export function useElementRect(propsuseElementRect: PropsUseElementRect): JSX.Element {
    return <UseElementRectModule {...propsuseElementRect} />
}