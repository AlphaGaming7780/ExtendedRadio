import { ModRegistrar } from "cs2/modding";
import { HelloWorldComponent } from "mods/hello-world";
import { StationsMenuExtend } from "mods/StationsMenuExtend";
import { StationDetailExtend } from "mods/StationDetailExtend";
import StationsMenuStyle from "./mods/Styles/StationsMenu.module.scss";
import StationDetailStyle from "./mods/Styles/StationDetail.module.scss";

const register: ModRegistrar = (moduleRegistry) => {

    moduleRegistry.extend('game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.module.scss', StationsMenuStyle);
    moduleRegistry.extend('game-ui/game/components/radio/radio-panel/station-detail/station-detail.module.scss', StationDetailStyle);

    moduleRegistry.extend('game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.tsx', 'StationsMenu', StationsMenuExtend);
    moduleRegistry.extend('game-ui/game/components/radio/radio-panel/station-detail/station-detail.tsx', 'StationDetail', StationDetailExtend);

    moduleRegistry.append('Menu', HelloWorldComponent);
}

export default register;