using System.Collections.Generic;
using System.Linq;
using Colossal.IO.AssetDatabase;
using Game.Audio.Radio;
using HarmonyLib;
using UnityEngine;
using static Game.Audio.Radio.Radio;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Game.UI.InGame;
using Game.Audio;
using System;
using Colossal.Randomization;
using Game.City;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace ExtendedRadio.Patches
{

    [HarmonyPatch(typeof( Radio ), "LoadRadio")]
    class Radio_LoadRadio {

        static void Postfix( Radio __instance) {

            ExtendedRadio.OnLoadRadio(__instance);

        }
    }

    //[HarmonyPatch(typeof( RadioUISystem ), "OnCreate")]
    //class RadioUISystem_OnCreate {
    //	static void Prefix(RadioUISystem __instance) {
    //		AudioManager.instance.radio.skipAds = Mod.m_Setting.DisableAdsOnStartup;
    //	}
    //}

    [HarmonyPatch(typeof( RadioUISystem ), "SelectStation", typeof(string))]
    class RadioUISystem_SelectStation {
        static void Postfix(string name) {
            if(ExtendedRadioMod._setting.SaveLastRadio) {
                ExtendedRadioMod._setting.LastRadio = name;
                ExtendedRadioMod._setting.ApplyAndSave();
            }
            ExtendedRadio.RadioStationChanged(name);
        }
    }

    [HarmonyPatch(typeof(AudioManager), "SetVolume")]
    class AudioManager_SetVolume
    {
        static void Postfix(string volumeProperty, float value)
        {
            if(volumeProperty == "RadioVolume") ExtendedRadio.RadioVolumeChanged(value);
        }
    }

    [HarmonyPatch(typeof(RadioPlayer), "Pause")]
    class RadioPlayer_Pause
    {
        static void Postfix()
        {
            ExtendedRadio.RadioPaused();
        }
    }

    [HarmonyPatch(typeof(RadioPlayer), "Unpause")]
    class RadioPlayer_Unpause
    {
        static void Postfix()
        {
            ExtendedRadio.RadioUnPaused();
        }
    }

    [HarmonyPatch(typeof(Radio), "NextSong")]
    class Radio_NextSong
    {
        static void Postfix()
        {
            ExtendedRadio.RadioNextSong();
        }
    }

    [HarmonyPatch(typeof(Radio), "PreviousSong")]
    class Radio_PreviousSong
    {
        static void Postfix()
        {
            ExtendedRadio.RadioPreviousSong();
        }
    }

    //[HarmonyPatch(typeof(AudioAsset), "LoadAsync")]
    //class AudioAsset_UpdateMetaTags
    //{
    //    static void Postfix(AudioAsset __instance)
    //    {
    //        Debug.Log($"UpdateMetaTags : {__instance.subPath}");
    //    }
    //}

    //   [HarmonyPatch(typeof(AudioAsset), "LoadAsync")]
    //internal class AudioAssetLoadAsyncPatch
    //{
    //	static bool Prefix(AudioAsset __instance, ref Task<AudioClip> __result)
    //	{	
    //		if(!CustomRadios.customeRadioChannelsName.Contains(__instance.GetMetaTag(AudioAsset.Metatag.RadioChannel))) return true;

    //		__result = LoadAudioFile(__instance);
    //		return false;
    //	}

    //	private static async Task<AudioClip> LoadAudioFile(AudioAsset audioAsset)
    //	{
    //		Traverse audioAssetTravers = Traverse.Create(audioAsset);

    //		if(audioAssetTravers.Field("m_Instance").GetValue() == null)
    //		{
    //			string sPath = MusicLoader.GetClipPathFromAudiAsset(audioAsset);
    //			using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + sPath, MusicLoader.GetClipFormatFromAudiAsset(audioAsset));
    //			((DownloadHandlerAudioClip) www.downloadHandler).streamAudio = true;
    //			await www.SendWebRequest();
    //			AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
    //			www.Dispose();

    //			clip.name = sPath;
    //			clip.hideFlags = HideFlags.DontSave;

    //			audioAssetTravers.Field("m_Instance").SetValue(clip);
    //		}

    //		return (AudioClip) audioAssetTravers.Field("m_Instance").GetValue();
    //	}
    //}


    [HarmonyPatch(typeof(Radio), "GetPlaylistClips")]
    class Radio_GetPlaylistClips
    {
        static bool Prefix(Radio __instance, RuntimeSegment segment)
        {
            if(__instance.currentChannel.network != MixNetwork.MixNetworkName) return true;
            IEnumerable<AudioAsset> assets = AssetDatabase.global.GetAssets<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((AudioAsset asset) => asset.ContainsTag(CustomRadios.FormatTagSegmentType(segment.type)) && MixNetwork.s_enabledTags[segment.type].Any(new Func<string, bool>(asset.ContainsTag))));
            List<AudioAsset> list = [.. assets];
            segment.clipsCap = assets.Count() > 10 ? 10 : assets.Count();
            System.Random rnd = new();
            List<int> list2 = (from x in Enumerable.Range(0, list.Count)
                               orderby rnd.Next()
                               select x).Take(segment.clipsCap).ToList<int>();
            AudioAsset[] array = new AudioAsset[segment.clipsCap];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = list[list2[i]];
            }
            segment.clips = array;
            return false;
        }
    }

    [HarmonyPatch(typeof(Radio), "GetCommercialClips")]
    class Radio_GetCommercialClips
    {
        static bool Prefix(Radio __instance, RuntimeSegment segment)
        {
            if (__instance.currentChannel.network != MixNetwork.MixNetworkName) return true;
            Dictionary<string, RadioNetwork>  m_Networks = ExtendedRadio.radioTravers.Field("m_Networks").GetValue<Dictionary<string, RadioNetwork>>();
            segment.clips = [];
            if (m_Networks.TryGetValue(__instance.currentChannel.network, out RadioNetwork radioNetwork) && radioNetwork.allowAds && MixNetwork.s_enabledTags.ContainsKey(segment.type))
            {
                WeightedRandom<AudioAsset> weightedRandom = [];
                Dictionary<string, List<AudioAsset>> dictionary = [];
                bool check(AudioAsset asset) => asset.ContainsTag(CustomRadios.FormatTagSegmentType(segment.type)) && MixNetwork.s_enabledTags[segment.type].Any(new Func<string, bool>(asset.ContainsTag));
                IEnumerable<AudioAsset> audioAssetList = AssetDatabase.global.GetAssets(SearchFilter<AudioAsset>.ByCondition(check));
                Debug.Log(audioAssetList.Count());
                foreach (AudioAsset audioAsset in audioAssetList)
                {
                    string metaTag = audioAsset.GetMetaTag(AudioAsset.Metatag.Brand);
                    if (metaTag != null)
                    {
                        if (!dictionary.TryGetValue(metaTag, out List<AudioAsset> list))
                        {
                            list = [];
                            dictionary.Add(metaTag, list);
                        }
                        list.Add(audioAsset);
                    }
                    else
                    {
                        ExtendedRadioMod.log.ErrorFormat("Asset {0} ({1}) does not contain a brand metatag (for Commercial segment)", audioAsset.guid, audioAsset.GetMetaTag(AudioAsset.Metatag.Title) ?? "<No title>");
                    }
                }
                NativeList<BrandPopularitySystem.BrandPopularity> brandPopularity = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BrandPopularitySystem>().ReadBrandPopularity(out JobHandle jobHandle);
                jobHandle.Complete();
                for (int i = 0; i < brandPopularity.Length; i++)
                {
                    if (World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>().TryGetPrefab<BrandPrefab>(brandPopularity[i].m_BrandPrefab, out BrandPrefab brandPrefab) && dictionary.TryGetValue(brandPrefab.name, out List<AudioAsset> key))
                    {
                        foreach (AudioAsset asset in key) Debug.Log(asset.name);
                        weightedRandom.AddRange(key, brandPopularity[i].m_Popularity);
                    }
                }
                List<AudioAsset> list2 = [];
                for (int j = 0; j < segment.clipsCap; j++)
                {
                    AudioAsset audioAsset2 = weightedRandom.NextAndRemove();
                    if (audioAsset2 != null)
                    {
                        list2.Add(audioAsset2);
                    }
                }
                segment.clips = list2;
            }

            return false;
        }
    }
}
