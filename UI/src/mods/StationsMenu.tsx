import { ModuleRegistryExtend } from "cs2/modding";

var executeOnce : boolean = true;

export const StationsMenu_ModuleRegistryExtend: ModuleRegistryExtend = (Component : any) => {	
	return (props) => {
		// translation handling. Translates using locale keys that are defined in C# or fallback string here.
		// const { translate } = useLocalization();
		
		// This defines aspects of the components.
		const { children, ...otherProps} = props || {};

		// console.log(Component())

		var stationsMenu : any = (
			<Component {...otherProps}>
				<div>HELLO WORLD</div>
				{children}
			</Component>
		);

		// console.log(stationsMenu)

		return stationsMenu
	};
}