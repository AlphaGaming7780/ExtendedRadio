import { getModule } from "cs2/modding"
import { Context, ContextType } from "react"

const path$ = "game-ui/common/foldout/foldout-context.ts"

export type PropsFoldoutContext = {
    theme: any;
    [key: string]: any;
}

export const FoldoutContext: Context<PropsFoldoutContext> = getModule(path$, "FoldoutContext")