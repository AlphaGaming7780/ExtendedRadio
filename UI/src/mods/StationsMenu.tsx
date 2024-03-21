import { useValue } from "cs2/api";
import { useLocalization } from "cs2/l10n";
import { ModuleRegistryExtend } from "cs2/modding";
import { Brush, Entity, tool } from "cs2/bindings"
import { Dropdown, DropdownItem, DropdownToggle, FOCUS_AUTO, FOCUS_DISABLED } from "cs2/ui";
import { createElement } from "react";
import { entityEquals, entityKey } from "cs2/utils";


export const StationsMenu: ModuleRegistryExtend = (Component : any) => {	
	return (props) => {
		// translation handling. Translates using locale keys that are defined in C# or fallback string here.
		// const { translate } = useLocalization();
		
		// This defines aspects of the components.
		const { children, ...otherProps} = props || {};

		// console.log(Component())

		var stationsMenu : any = (
            <Component {...otherProps}>
				{children}
            </Component>
        );

		console.log(stationsMenu)

		return stationsMenu
	};
}