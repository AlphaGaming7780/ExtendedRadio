import { ModuleRegistryExtend } from "cs2/modding";
import { RadioNetwork, UISound, prefab, radio } from "cs2/bindings";
import { trigger, useValue } from "cs2/api";
import { StationsMenuSCSS } from "../../game-ui/game/components/radio/radio-panel/stations-menu/stations-menu.module.scss";
import classNames from "classnames";
import { TutorialTarget } from "../../game-ui/game/components/tutorials/tutorial-target/tutorial-target";
import { Scrollable, Tooltip } from "cs2/ui";
import { TooltipLayout } from "../../game-ui/common/tooltip/description-tooltip/description-tooltip";
import { useLocalization } from "cs2/l10n";
import { IconButton } from "../../game-ui/common/input/button/icon-button";
import ExtendedStationsMenuSCSS from "./Styles/StationsMenu.module.scss";

export const StationsMenuExtend: ModuleRegistryExtend = (Component: any) => {
	return (props) => {
		//translation handling. Translates using locale keys that are defined in C# or fallback string here.
		const { translate } = useLocalization();
		let selectedNetwork: string | null = useValue(radio.selectedNetwork$);
		let networks: RadioNetwork[] = useValue(radio.networks$)
		let manualUITags: prefab.ManualUITagsConfiguration | null = useValue(prefab.manualUITags$)

		// This defines aspects of the components.
		const { children, ...otherProps } = props || {};
		const radio$ = "radio";

		function Fse(radioNetwork: RadioNetwork, selected: boolean): JSX.Element {
			function OnSelect() { trigger(radio$, "selectNetwork", radioNetwork.name) }

			var title = radioNetwork.nameId ? translate(radioNetwork.nameId) : radioNetwork.name
			var description = radioNetwork.descriptionId ? translate(radioNetwork.descriptionId) : radioNetwork.description

			return <Tooltip {...{ tooltip: TooltipLayout({ title: title, description: description },) }}>
				{
					<IconButton
						src={radioNetwork.icon}
						className={classNames(StationsMenuSCSS.networkItem, ExtendedStationsMenuSCSS.networkItem)}
						selectSound={UISound?.selectRadioNetwork}
						onSelect={OnSelect}
						selected={selected}
					/>
				}
			</Tooltip>
		}
		
		if (selectedNetwork === "Mix Network") {
			return <div className={classNames(StationsMenuSCSS.stationsMenu, otherProps.className, ExtendedStationsMenuSCSS.stationsMenu)} style={{ width: "74.75rem" }} >
				<TutorialTarget uiTag={manualUITags?.radioPanelNetworks} >
					<div className={classNames(StationsMenuSCSS.networks, ExtendedStationsMenuSCSS.networks)} >
						{
							networks.map((radioNetwork) => { return Fse(radioNetwork, radioNetwork.name === selectedNetwork) })
						}
					</div>
				</TutorialTarget>
			</div>
		} else {
			return <Component classNames={classNames(ExtendedStationsMenuSCSS.stationsMenu, otherProps.className) }> {children} </Component>
		}
	};
}