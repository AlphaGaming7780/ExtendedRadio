using System;
using System.Collections;
using System.IO;
using Colossal.UI.Binding;
using Game.SceneFlow;
using Game.UI;
using UnityEngine;

namespace ExtendedRadio.UI
{
	
	partial class ExtendedRadioUI : UISystemBase
	{	
		internal static  GameObject extendedRadioGameObject = new();
		private GetterValueBinding<bool> DisableAdsOnStartup;
		private GetterValueBinding<bool> SaveLastRadio;

        protected override void OnCreate() {

			base.OnCreate();
			
			AddBinding(DisableAdsOnStartup = new GetterValueBinding<bool>("extended_radio_settings", "DisableAdsOnStartup", () => Mod.m_Setting.DisableAdsOnStartup /*Settings.DisableAdsOnStartup*/));
			AddBinding(new TriggerBinding<bool>("extended_radio_settings", "DisableAdsOnStartup", new Action<bool>(UpdateSettings_disableAdsOnStartup)));

			AddBinding(SaveLastRadio = new GetterValueBinding<bool>("extended_radio_settings", "SaveLastRadio", () => Mod.m_Setting.SaveLastRadio /* Settings.SaveLastRadio*/));
			AddBinding(new TriggerBinding<bool>("extended_radio_settings", "SaveLastRadio", new Action<bool>(UpdateSettings_saveLastRadio)));

			AddBinding(new TriggerBinding("extended_radio", "reloadradio", new Action(ReloadRadio)));
        }

		private void UpdateSettings_disableAdsOnStartup(bool newValue) {
			Mod.m_Setting.DisableAdsOnStartup = newValue;
			Mod.m_Setting.ApplyAndSave();
			DisableAdsOnStartup.Update();
		}

		private void UpdateSettings_saveLastRadio(bool newValue) {
			Mod.m_Setting.SaveLastRadio = newValue;
			if(newValue) Mod.m_Setting.LastRadio = ExtendedRadio.radio.currentChannel.name;
			Mod.m_Setting.ApplyAndSave();
			SaveLastRadio.Update();
		}

		private void ReloadRadio() {
			ExtendedRadio.radio.Reload(true);
		}

		internal static string GetStringFromEmbbededJSFile(string path) {
			return new StreamReader(ExtendedRadio.GetEmbedded("UI."+path)).ReadToEnd();
		}

	}
}