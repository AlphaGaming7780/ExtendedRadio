import { Typed } from "cs2/bindings"
import { getModule } from "cs2/modding"

const path$ = "game-ui/common/typed-renderer/typed-renderer.tsx"

export type PropsTypedListRenderer = { components: { [x: string]: ( {} : any ) => any } , data: Typed<"">[], props: any, keyProvider?: any }
export type PropsTypedRenderer = { components: { [x: string]: ({ }: any) => any }, data: Typed<"">, props: any }

export function TypedRenderer(propsDescriptionTooltip: PropsTypedRenderer): JSX.Element {
    return getModule(path$, "TypedRenderer")(propsDescriptionTooltip)
}

export function TypedListRenderer(propsDescriptionTooltip: PropsTypedListRenderer): JSX.Element {   
    return getModule(path$, "TypedListRenderer")(propsDescriptionTooltip)
}