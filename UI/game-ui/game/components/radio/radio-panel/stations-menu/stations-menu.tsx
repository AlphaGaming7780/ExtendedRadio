import { getModule } from "cs2/modding"

const path$ = "game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.tsx"

export type PropsStationsMenu = {
    className? : string
}

export function StationsMenu(propsStationsMenu: PropsStationsMenu) : JSX.Element
{
    return getModule(path$, "StationsMenu")(propsStationsMenu)
}
