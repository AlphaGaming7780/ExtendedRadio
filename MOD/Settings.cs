using Colossal.IO.AssetDatabase;
using Game.Input;
using Game.Modding;
using Game.Settings;
using System.Collections.Generic;
using static Game.Audio.Radio.Radio;

namespace ExtendedRadio;

[FileLocation($"ModsSettings\\{nameof(ExtendedRadio)}\\settings")]
[SettingsUIGroupOrder(kQOLGroup, kUtilityGroup)]
[SettingsUIShowGroupName(kQOLGroup, kUtilityGroup)]
public class Setting(IMod mod) : ModSetting(mod)
{
	public const string kMainSection = "Main";
    public const string kQOLGroup = "QOL";
    public const string kUtilityGroup = "Utility";

    [SettingsUISection(kMainSection, kQOLGroup)]
	public bool DisableAdsOnStartup { get; set; } = false;

	[SettingsUISection(kMainSection, kQOLGroup)]
	public bool SaveLastRadio { get; set; } = true;

    [SettingsUIButton]
    [SettingsUISection(kMainSection, kUtilityGroup)]
    public bool ReloadRadio { set {if(ExtendedRadio.radio != null && ExtendedRadio.radio.isEnabled) ExtendedRadio.radio.Reload(true);} }

    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUISection(kMainSection, kUtilityGroup)]
    public bool ResetSettings { set {SetDefaults();} }



    public const string kMixNetworkSection = "MixNetwork";
    public const string kMixNetworkGroup = "MixNetwork";
    [SettingsUISection(kMixNetworkSection, kMixNetworkGroup)]
    public bool MixNetworkEnabled { get; set; } = true;

    [SettingsUISection(kMixNetworkSection, kMixNetworkGroup)]
    [SettingsUIDisableByConditionAttribute(typeof(Setting), nameof(DisableCondition_MixNetworkClearQueue))]
    public bool MixNetworkClearQueue { get; set; } = true;
    public bool DisableCondition_MixNetworkClearQueue => !MixNetworkEnabled;

    [SettingsUISection(kMixNetworkSection, kMixNetworkGroup)]
    [SettingsUIDisableByConditionAttribute(typeof(Setting), nameof(DisableCondition_MixNetworkFinishCurrentClip))]
    public bool MixNetworkFinishCurrentClip { get; set; } = true;
    public bool DisableCondition_MixNetworkFinishCurrentClip => !MixNetworkEnabled || !MixNetworkClearQueue;

    public Dictionary<SegmentType, List<string>> EnabledTags = [];


    public const string kKeybindsSection = "Keybinds";
    public const string kRadioControlsGroup = "RadioControls";

    [SettingsUISection(kKeybindsSection, kRadioControlsGroup)]
    [SettingsUIKeyboardBinding(BindingKeyboard.Home, actionName: nameof(PauseRadioBinding), alt: true)]
    public ProxyBinding PauseRadioBinding { get; set; }

    [SettingsUISection(kKeybindsSection, kRadioControlsGroup)]
    [SettingsUIKeyboardBinding(BindingKeyboard.End, actionName: nameof(MuteRadioBinding), alt: true)]
    public ProxyBinding MuteRadioBinding { get; set; }

    [SettingsUISection(kKeybindsSection, kRadioControlsGroup)]
    [SettingsUIKeyboardBinding(BindingKeyboard.PageUp, actionName: nameof(NextSongRadioBinding), alt: true)]
    public ProxyBinding NextSongRadioBinding { get; set; }

    // I wish the INSERT key was in
    [SettingsUISection(kKeybindsSection, kRadioControlsGroup)]
    [SettingsUIKeyboardBinding(BindingKeyboard.None, actionName: nameof(PrevSongRadioBinding), alt: true)]
    public ProxyBinding PrevSongRadioBinding { get; set; }

    [SettingsUISection(kKeybindsSection, kRadioControlsGroup)]
    [SettingsUIKeyboardBinding(BindingKeyboard.Delete, actionName: nameof(VolumeDownRadioBinding), alt: true)]
    public ProxyBinding VolumeDownRadioBinding { get; set; }

    [SettingsUISection(kKeybindsSection, kRadioControlsGroup)]
    [SettingsUIKeyboardBinding(BindingKeyboard.PageDown, actionName: nameof(VolumeUpRadioBinding), alt: true)]
    public ProxyBinding VolumeUpRadioBinding { get; set; }

    public string LastRadio = null;

    public override void SetDefaults()
	{	
		DisableAdsOnStartup = false;
		SaveLastRadio = true;
		LastRadio = null;
        ResetKeyBindings();
    }
}