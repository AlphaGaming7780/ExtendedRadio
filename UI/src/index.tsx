import { ModRegistrar } from "cs2/modding";
import { HelloWorldComponent } from "mods/hello-world";
import { StationsMenu } from "mods/StationsMenu";
import StationsMenuStyle from "./mods/StationsMenu.module.scss";
import StationDetailStyle from "./mods/StationDetail.module.scss";
import { bindValue, useValue } from "cs2/api";

const register: ModRegistrar = (moduleRegistry) => {

    moduleRegistry.extend('game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.module.scss', StationDetailStyle);
    moduleRegistry.extend('game-ui/game/components/radio/radio-panel/station-detail/station-detail.module.scss', StationDetailStyle);

    moduleRegistry.extend('game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.tsx', 'StationsMenu', StationsMenu);

    moduleRegistry.append('Menu', HelloWorldComponent);
}

export default register;