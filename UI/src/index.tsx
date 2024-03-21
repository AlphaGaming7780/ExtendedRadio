import { ModRegistrar } from "cs2/modding";
import { HelloWorldComponent } from "mods/hello-world";
import { StationsMenu_ModuleRegistryExtend } from "mods/StationsMenu";
import StationsMenuStyle from "./mods/StationsMenu.module.scss";
import StationDetailStyle from "./mods/StationDetail.module.scss";

const register: ModRegistrar = (moduleRegistry) => {

    moduleRegistry.extend('game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.module.scss', StationsMenuStyle);
    moduleRegistry.extend('game-ui/game/components/radio/radio-panel/station-detail/station-detail.module.scss', StationDetailStyle);

    // moduleRegistry.extend('game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.tsx', 'StationsMenu', StationsMenu_ModuleRegistryExtend);

    moduleRegistry.append('Menu', HelloWorldComponent);
}

export default register;