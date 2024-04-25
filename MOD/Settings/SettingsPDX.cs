using System.Collections.Generic;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI.Widgets;

namespace ExtendedRadio;

[FileLocation($"ModSettings\\{nameof(ExtendedRadio)}\\settings")]
[SettingsUIGroupOrder(kQOLGroup, kUtilityGroup)]
[SettingsUIShowGroupName(kQOLGroup, kUtilityGroup)]
public class Setting(IMod mod) : ModSetting(mod)
{
		public const string kSection = "Main";
		public const string kQOLGroup = "QOL";
		public const string kUtilityGroup = "Utility";

		[SettingsUISection(kSection, kQOLGroup)]
		public bool DisableAdsOnStartup { get; set; } = false;

		[SettingsUISection(kSection, kQOLGroup)]
		public bool SaveLastRadio { get; set; } = true;

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
	}
}

//public class LocaleEN(Setting setting) : IDictionarySource
//{
//	private readonly Setting m_Setting = setting;

//	public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
//	{
//		return new Dictionary<string, string>
//		{
//			{ m_Setting.GetSettingsLocaleID(), "ExtendedRadio" },
//			{ m_Setting.GetOptionTabLocaleID(Setting.kSection), "Main" },

//			{ m_Setting.GetOptionGroupLocaleID(Setting.kQOLGroup), "Quality Of Life" },
//			{ m_Setting.GetOptionGroupLocaleID(Setting.kUtilityGroup), "Utility" },

//			{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.DisableAdsOnStartup)), "Disable ads on startup"},
//			{ m_Setting.GetOptionDescLocaleID(nameof(Setting.DisableAdsOnStartup)), $"Disable the ads when you load into a game." },

//			{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.SaveLastRadio)), "Load last radio on startup"},
//			{ m_Setting.GetOptionDescLocaleID(nameof(Setting.SaveLastRadio)), $"Select the last selected radio when you load into a game." },

//			{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.ReloadRadio)), "Reload radios"},
//			{ m_Setting.GetOptionDescLocaleID(nameof(Setting.ReloadRadio)), $"Reload the radios, can maybe update your radio if you made change to it. Can also crash the game." },

//			{ m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetSettings)), "Reset ExtendedRadio settings" },
//			{ m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetSettings)), $"Reset the settings of the mod to the default one." },
//			{ m_Setting.GetOptionWarningLocaleID(nameof(Setting.ResetSettings)), "Are you sur you want to reset the settings of ExtendedRadio?" },
//		};
//	}

//	public void Unload()
//	{

//	}
//}