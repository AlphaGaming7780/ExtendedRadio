﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Colossal.IO.AssetDatabase;
using Game.Audio.Radio;
using Game.SceneFlow;
using Game.UI;
using HarmonyLib;
using UnityEngine;
using static Game.Audio.Radio.Radio;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Game.UI.InGame;
using Game.Common;
using Game.Audio;

namespace ExtendedRadio.Patches
{

    [HarmonyPatch(typeof(SystemOrder), "Initialize")]
    public static class SystemOrderPatch {
        public static void Postfix(Game.UpdateSystem updateSystem) {
            updateSystem.UpdateAt<ExtendedRadioUI>(Game.SystemUpdatePhase.UIUpdate);
        }
    }


	[HarmonyPatch(typeof(GameManager), "InitializeThumbnails")]
	internal class GameManager_InitializeThumbnails
	{	
		internal static  GameObject extendedRadioGameObject = new();
		internal static ExtendedRadioUI_Mono extendedRadioUi;
		static readonly string IconsResourceKey = $"{MyPluginInfo.PLUGIN_NAME.ToLower()}";

		public static readonly string COUIBaseLocation = $"coui://{IconsResourceKey}";

		static internal readonly string resources = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "resources");
		public static readonly string CustomRadiosPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CustomRadios");
		static private readonly string PathToParent = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName;
		public static readonly string PathToMods = Path.Combine(PathToParent,"ExtendedRadio_mods");
		public static readonly string ModsFolderCustomRadio = Path.Combine(PathToMods,"CustomRadios");
		public static readonly string ModsFolderRadioAddons = Path.Combine(PathToMods,"RadioAddons");

		static void Prefix(GameManager __instance)
		{		

			List<string> pathToIconToLoad = [Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)];

			Directory.CreateDirectory(CustomRadiosPath);
			CustomRadios.RegisterCustomRadioDirectory(CustomRadiosPath);

			if(!Directory.Exists(resources)) {
				Directory.CreateDirectory(resources);
				File.Move(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DefaultIcon.svg"), Path.Combine(resources , "DefaultIcon.svg"));
			}

			if(Directory.Exists(ModsFolderCustomRadio)) {
				CustomRadios.RegisterCustomRadioDirectory(ModsFolderCustomRadio);
				pathToIconToLoad.Add(PathToMods);
			}

			if(Directory.Exists(ModsFolderRadioAddons)) {
				RadioAddons.RegisterRadioAddonsDirectory(ModsFolderRadioAddons);
			}

			var gameUIResourceHandler = (GameUIResourceHandler)GameManager.instance.userInterface.view.uiSystem.resourceHandler;
			
			if (gameUIResourceHandler == null)
			{
				Debug.LogError("Failed retrieving GameManager's GameUIResourceHandler instance, exiting.");
				return;
			}
			
			gameUIResourceHandler.HostLocationsMap.Add(
				IconsResourceKey, pathToIconToLoad
			);

			extendedRadioUi = extendedRadioGameObject.AddComponent<ExtendedRadioUI_Mono>();

		}
	}

	[HarmonyPatch(typeof( Radio ), "LoadRadio")]
	class Radio_LoadRadio {

		static void Postfix( Radio __instance) {

			ExtendedRadio.OnLoadRadio(__instance);

		}
	}

	[HarmonyPatch(typeof( RadioUISystem ), "OnCreate")]
	class RadioUISystem_OnCreate {
		static void Prefix(RadioUISystem __instance) {
			Settings.LoadSettings();
			AudioManager.instance.radio.skipAds = Settings.DisableAdsOnStartup;
		}
	}

	[HarmonyPatch(typeof(AudioAsset), "LoadAsync")]
	internal class AudioAssetLoadAsyncPatch
	{
		static bool Prefix(AudioAsset __instance, ref Task<AudioClip> __result)
		{	
			if(!CustomRadios.customeRadioChannelsName.Contains(__instance.GetMetaTag(AudioAsset.Metatag.RadioChannel))) return true;
			
			__result = LoadAudioFile(__instance);
			return false;
		}

		private static async Task<AudioClip> LoadAudioFile(AudioAsset audioAsset)
		{
			Traverse audioAssetTravers = Traverse.Create(audioAsset);

			if(audioAssetTravers.Field("m_Instance").GetValue() == null)
			{
				string sPath = MusicLoader.GetClipPathFromAudiAsset(audioAsset);
				using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + sPath, MusicLoader.GetClipFormatFromAudiAsset(audioAsset));
				((DownloadHandlerAudioClip) www.downloadHandler).streamAudio = true;
				await www.SendWebRequest();
				AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
				www.Dispose();

				clip.name = sPath;
				clip.hideFlags = HideFlags.DontSave;

				audioAssetTravers.Field("m_Instance").SetValue(clip);
			}

			return (AudioClip) audioAssetTravers.Field("m_Instance").GetValue();
		}
	}


	[HarmonyPatch( typeof( Radio ), "GetPlaylistClips" )]
	class Radio_GetPlaylistClips
	{
		static bool Prefix( Radio __instance, RuntimeSegment segment)
		{		
			if(CustomRadios.customeRadioChannelsName.Contains(__instance.currentChannel.name)) {

				IEnumerable<AudioAsset> assets = ExtendedRadio.GetAudioAssetsFromAudioDataBase(__instance, segment.type);
				List<AudioAsset> list = [.. assets];
				System.Random rnd = new();
				List<int> list2 = (from x in Enumerable.Range(0, list.Count)
								orderby rnd.Next()
								select x).Take(segment.clipsCap).ToList();
				AudioAsset[] array = new AudioAsset[segment.clipsCap];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = list[list2[i]];
				}

				segment.clips = array;

				return false;
			}
			return true;
		}
	}

	[HarmonyPatch( typeof( Radio ), "GetCommercialClips" )]
	class Radio_GetCommercialClips
	{
        static bool Prefix( Radio __instance, RuntimeSegment segment)
		{
			if(CustomRadios.customeRadioChannelsName.Contains(__instance.currentChannel.name)) {


				Dictionary<string, RadioNetwork> m_Networks = Traverse.Create(__instance).Field("m_Networks").GetValue<Dictionary<string, RadioNetwork>>();

				if (!m_Networks.TryGetValue(__instance.currentChannel.network, out var value) || !value.allowAds)
				{	
					return false;
				}

				IEnumerable<AudioAsset> assets = ExtendedRadio.GetAudioAssetsFromAudioDataBase(__instance, segment.type);
				List<AudioAsset> list = [.. assets];
				System.Random rnd = new();
				List<int> list2 = (from x in Enumerable.Range(0, list.Count)
								orderby rnd.Next()
								select x).Take(segment.clipsCap).ToList();
				AudioAsset[] array = new AudioAsset[segment.clipsCap];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = list[list2[i]];
				}

				segment.clips = array;

				return false;
			}
			return true;
		}
	}
	
	[HarmonyPatch( typeof( GamePanelUISystem ), "ShowPanel", typeof(GamePanel) )]
	class GamePanelUISystem_TogglePanel : UISystemBase 
	{
		static void Postfix( GamePanelUISystem __instance, GamePanel panel) {
			if(panel.GetType().ToString() == "Game.UI.InGame.RadioPanel") {

				GameManager_InitializeThumbnails.extendedRadioUi.ChangeUiNextFrame(ExtendedRadioUI.GetStringFromEmbbededJSFile("Setup.js"));

				GameManager_InitializeThumbnails.extendedRadioUi.ChangeUiNextFrame(ExtendedRadioUI.GetStringFromEmbbededJSFile("ExtendedRadioSettings.js"));
				if(Settings.customNetworkUI) {
					GameManager_InitializeThumbnails.extendedRadioUi.ChangeUiNextFrame(ExtendedRadioUI.GetStringFromEmbbededJSFile("RadioNetworkFix.js"));
				}
			}
		}
	}
}
