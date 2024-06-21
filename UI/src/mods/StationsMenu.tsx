import { ModuleRegistryExtend } from "cs2/modding";
import { RadioNetwork, radio } from "cs2/bindings";
import { useValue } from "cs2/api";
import { StationsMenuSCSS } from "../../game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.module.scss";
import classNames from "classnames";

export const StationsMenu_ModuleRegistryExtend: ModuleRegistryExtend = (Component : any) => {	
	return (props) => {
		// translation handling. Translates using locale keys that are defined in C# or fallback string here.
		// const { translate } = useLocalization();

		// This defines aspects of the components.
		const { children, ...otherProps } = props || {};


		let selectedNetwork: string | null = useValue(radio.selectedNetwork$);
		let networks: RadioNetwork[] = useValue(radio.networks$)

		console.log(selectedNetwork);
		
		if (selectedNetwork == "Mix") {
			return <div className={classNames(StationsMenuSCSS.stationsMenu, ...otherProps)} >
				<div className={ StationsMenuSCSS.networks }>
					{
						networks.map((network) => { return <div>{network.name}</div>})
					}
				</div>
			</div>
		} else {
			return <Component {...otherProps}> {/*{children}*/} </Component>
		}
	};
}