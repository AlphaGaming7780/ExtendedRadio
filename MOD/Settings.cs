using Colossal.IO.AssetDatabase;
using Game.Input;
using Game.Modding;
using Game.Settings;

namespace ExtendedRadio;

[FileLocation($"ModsSettings\\{nameof(ExtendedRadio)}\\settings")]
[SettingsUIGroupOrder(kQOLGroup, kKeybinds, kUtilityGroup)]
[SettingsUIShowGroupName(kQOLGroup, kKeybinds, kUtilityGroup)]
public class Setting(IMod mod) : ModSetting(mod)
{
	public const string kSection = "Main";
	public const string kQOLGroup = "QOL";
    public const string kKeybinds = "Keybinds";
    public const string kUtilityGroup = "Utility";

	[SettingsUISection(kSection, kQOLGroup)]
	public bool DisableAdsOnStartup { get; set; } = false;

	[SettingsUISection(kSection, kQOLGroup)]
	public bool SaveLastRadio { get; set; } = true;

    [SettingsUISection(kSection, kKeybinds)]
    [SettingsUIKeyboardBinding(UnityEngine.InputSystem.Key.None, actionName: "PauseRadioBinding")]
    public ProxyBinding PauseRadioBinding { get; set; }

    [SettingsUIButton]
    [SettingsUISection(kSection, kUtilityGroup)]
    public bool ReloadRadio { set {if(ExtendedRadio.radio != null && ExtendedRadio.radio.isEnabled) ExtendedRadio.radio.Reload(true);} }

    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUISection(kSection, kUtilityGroup)]
    public bool ResetSettings { set {SetDefaults();} }

	public string LastRadio = null;

    public override void SetDefaults()
	{	
		DisableAdsOnStartup = false;
		SaveLastRadio = true;
		LastRadio = null;
        ResetKeyBindings();
    }
}