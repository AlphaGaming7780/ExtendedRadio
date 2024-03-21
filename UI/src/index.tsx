import { ModRegistrar } from "cs2/modding";
import { HelloWorldComponent } from "mods/hello-world";
// import { StationsMenu } from "mods/StationsMenu";
import StationsMenu from "./mods/StationsMenu.module.scss";
import StationDetail from "./mods/StationDetail.module.scss";
import { bindValue, useValue } from "cs2/api";

const register: ModRegistrar = (moduleRegistry) => {

        moduleRegistry.extend('game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.module.scss', StationsMenu);
        moduleRegistry.extend('game-ui/game/components/radio/radio-panel/station-detail/station-detail.module.scss', StationDetail);

    // moduleRegistry.extend('game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.tsx', 'StationsMenu', StationsMenu);

    moduleRegistry.append('Menu', HelloWorldComponent);
}

export default register;