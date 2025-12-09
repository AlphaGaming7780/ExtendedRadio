import { ModuleRegistryExtend } from "cs2/modding";
import { RadioNetwork, prefab, radio } from "cs2/bindings";
import { bindValue, trigger, useValue } from "cs2/api";
import { Scrollable, Tooltip } from "cs2/ui";
import { FoldoutItem, FoldoutItemHeader } from "../../game-ui/common/foldout/foldout-item";
import { StationDetailSCSS } from "../../game-ui/game/components/radio/radio-panel/station-detail/station-detail.module.scss";
import { StatisticsItemSCSS } from "../../game-ui/game/components/statistics-panel/menu/item/statistics-item.module.scss";
import { Checkbox } from "../../game-ui/common/input/toggle/checkbox/checkbox";
import { StatisticsCheckboxSCSS } from "../../game-ui/game/components/statistics-panel/menu/item/statistics-checkbox.module.scss";
import MixNetworkSCSS from "../mods/Styles/MixNetworks.module.scss";
import { FoldoutItemSCSS } from "../../game-ui/common/foldout/foldout-item.module.scss";

export type RadioTag = {
	Name: string
	Tag: string
	Type: string
	RadioTags: RadioTag[]
}

export const radioTags$ = bindValue <RadioTag[]>("extended_radio_mix", 'radiotags');
export const enabledTags$: any = bindValue("extended_radio_mix", 'enabledtags');

export const StationDetailExtend: ModuleRegistryExtend = (Component: any) => {
	return (props) => {

		let selectedNetwork: string | null = useValue(radio.selectedNetwork$);
		//let radioTags: Array<RadioTag> = [{ Tag: "Element 1", Selected: false, RadioTags: [{ Tag: "Element 1.1", Selected: false, RadioTags: [] }] }, { Tag: "Element 2", Selected: false, RadioTags: [{ Tag: "Element 2.1", Selected: false, RadioTags: [] }] }  ]
		let radioTags: RadioTag[] = useValue(radioTags$)
		let enabledTags: any = useValue(enabledTags$);

		function AddTag(tag : string, type : string) { trigger("extended_radio_mix", "addtag", tag, type) }
		function RemoveTag(tag: string, type: string) { trigger("extended_radio_mix", "removetag", tag, type) }

		function Selected(radioTag: RadioTag) {
			return enabledTags[radioTag.Type] && enabledTags[radioTag.Type].includes(radioTag.Tag, 0)
		}

		function FoldoutRadioItemHeader(radioTag: RadioTag, parentChecked: boolean): JSX.Element {

			function onCheckBoxChange(value : boolean) {
				if (value) {
					AddTag(radioTag.Tag, radioTag.Type)
				} else {
					RemoveTag(radioTag.Tag, radioTag.Type)
				}
			}

			return <FoldoutItemHeader>
				<span className={MixNetworkSCSS.FoldoutItemHeaderSpan}>
					<Checkbox {...{
						theme: StatisticsCheckboxSCSS,
						disabled: parentChecked,
						onChange: onCheckBoxChange,
						checked: parentChecked || Selected(radioTag),
						className: MixNetworkSCSS.checkBox
					}} />
					<div className={StatisticsItemSCSS.label} style={{ color: "rgba(80, 76, 83, 1)" }}>
						{radioTag.Name}
					</div>
				</span>
			</FoldoutItemHeader>
		}

		function FoldoutRadioItem(radioTag: RadioTag, nesting: number = 0, parentChecked: boolean): JSX.Element {
			const theme2 = { ...FoldoutItemSCSS, ...MixNetworkSCSS }
			const selected = Selected(radioTag)
			return <FoldoutItem
				header={FoldoutRadioItemHeader(radioTag, parentChecked)}
				initialExpanded={ parentChecked || selected }
				expandFromContent={false}
				type={radioTag.RadioTags.length > 0 ? "Group" : "Item"}
				nesting={nesting}
				theme={theme2} >
				{
					radioTag.RadioTags.length > 0 ? radioTag.RadioTags.map((radioTag2: RadioTag) =>
					{
						return FoldoutRadioItem(radioTag2, nesting + 1, selected || parentChecked)
					}) : null
				}
			</FoldoutItem>
		}

		if (selectedNetwork === "Mix Network") {
			return <div className={StationDetailSCSS.stationDetail} style={{ width: "685.25rem", height: "100%",  }}>
				<Scrollable>
					{
						radioTags.map((radioTag: RadioTag) => {
							return FoldoutRadioItem(radioTag, 0, false )
						})
					}
				</Scrollable>
			</div> 
		} else {
			return <Component {...props}> </Component>
		}
	};
}