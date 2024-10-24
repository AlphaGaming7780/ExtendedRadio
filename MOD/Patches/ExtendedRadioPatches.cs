using System.Collections.Generic;
using System.Linq;
using Colossal.IO.AssetDatabase;
using Game.Audio.Radio;
using HarmonyLib;
using UnityEngine;
using static Game.Audio.Radio.Radio;
using Game.UI.InGame;
using Game.Audio;
using System;
using Colossal.Randomization;
using Game.City;
using Game.Prefabs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Game.Triggers;
using System.IO;
using System.Threading.Tasks;
using Game.Simulation;

namespace ExtendedRadio.Patches
{
    class ExtendedRadioPatch
    {
        [HarmonyPatch(typeof(Radio), "LoadRadio")]
        class Radio_LoadRadio
        {

            static void Postfix(Radio __instance)
            {

                ExtendedRadio.OnLoadRadio(__instance);

            }
        }

        [HarmonyPatch(typeof(Radio), "Enable")]
        class Radio_Enable
        {

            static void Postfix(Radio __instance)
            {

                __instance.skipAds = ExtendedRadioMod.s_setting.DisableAdsOnStartup;

                if (ExtendedRadioMod.s_setting.SaveLastRadio && ExtendedRadioMod.s_setting.LastRadio != null )
                {
                    RuntimeRadioChannel radio = __instance.GetRadioChannel(ExtendedRadioMod.s_setting.LastRadio);
                    if (radio != null)
                    {
                        __instance.currentChannel = radio;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(RadioUISystem), "SelectStation", typeof(string))]
        class RadioUISystem_SelectStation
        {
            static void Postfix(string name)
            {
                if (ExtendedRadioMod.s_setting.SaveLastRadio)
                {
                    ExtendedRadioMod.s_setting.LastRadio = name;
                    ExtendedRadioMod.s_setting.ApplyAndSave();
                }
                ExtendedRadio.RadioStationChanged(name);
            }
        }

        [HarmonyPatch(typeof(AudioManager), "SetVolume")]
        class AudioManager_SetVolume
        {
            static void Postfix(string volumeProperty, float value)
            {
                if (volumeProperty == "RadioVolume") ExtendedRadio.RadioVolumeChanged(value);
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

        [HarmonyPatch(typeof(AudioAsset), "LoadAsync")]
        internal class AudioAssetLoadAsyncPatch
        {
            static bool Prefix(AudioAsset __instance, ref Task<AudioClip> __result, bool useCached = true, AudioType audioType = AudioType.OGGVORBIS)
            {
                __result = __instance.LoadAsyncFile(useCached, MusicLoader.GetClipFormatFromFileExtension(Path.GetExtension(__instance.path)));
                return false;
            }
        }


        [HarmonyPatch(typeof(Radio), "GetPlaylistClips")]
        class Radio_GetPlaylistClips
        {
            static bool Prefix(Radio __instance, RuntimeSegment segment)
            {
                if (__instance.currentChannel.network != MixNetwork.MixNetworkName) return true;
                IEnumerable<AudioAsset> assets = AssetDatabase.global.GetAssets<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((AudioAsset asset) => asset.ContainsTag(CustomRadios.FormatTagSegmentType(segment.type)) && MixNetwork.s_enabledTags.ContainsKey(segment.type) && MixNetwork.s_enabledTags[segment.type].Any(new Func<string, bool>(asset.ContainsTag))));
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
                Dictionary<string, RadioNetwork> m_Networks = ExtendedRadio.radioTravers.Field("m_Networks").GetValue<Dictionary<string, RadioNetwork>>();
                segment.clips = [];
                if (m_Networks.TryGetValue(__instance.currentChannel.network, out RadioNetwork radioNetwork) && radioNetwork.allowAds && MixNetwork.s_enabledTags.ContainsKey(segment.type))
                {
                    WeightedRandom<AudioAsset> weightedRandom = [];
                    Dictionary<string, List<AudioAsset>> dictionary = [];
                    bool check(AudioAsset asset) => asset.ContainsTag(CustomRadios.FormatTagSegmentType(segment.type)) && MixNetwork.s_enabledTags[segment.type].Any(new Func<string, bool>(asset.ContainsTag));
                    IEnumerable<AudioAsset> audioAssetList = AssetDatabase.global.GetAssets(SearchFilter<AudioAsset>.ByCondition(check));
                    segment.clipsCap = audioAssetList.Count() > 1 ? 1 : audioAssetList.Count();
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
                            try
                            {
                                weightedRandom.AddRange(key, brandPopularity[i].m_Popularity);
                            }
                            catch {}
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

        [HarmonyPatch(typeof(Radio), "GetPSAClips")]
        class Radio_GetPSAClips
        {
            static bool Prefix(Radio __instance, RuntimeSegment segment)
            {
                if (__instance.currentChannel.network != MixNetwork.MixNetworkName) return true;
                Dictionary<string, RadioNetwork> m_Networks = ExtendedRadio.radioTravers.Field("m_Networks").GetValue<Dictionary<string, RadioNetwork>>();
                segment.clips = [];
                if (m_Networks.TryGetValue(__instance.currentChannel.network, out RadioNetwork radioNetwork) && !radioNetwork.allowAds && MixNetwork.s_enabledTags.ContainsKey(segment.type))
                {
                    segment.clips = GetEventClips(__instance, segment, AudioAsset.Metatag.PSAType, false, false);
                }
                return false;
            }
        }


        [HarmonyPatch(typeof(Radio), "GetEventClips")]
        class Radio_GetEventClips
        {
            static bool Prefix(Radio __instance, ref List<AudioAsset> __result, RuntimeSegment segment, AudioAsset.Metatag metatag, bool newestFirst = false, bool flush = false)
            {
                if (__instance.currentChannel.network != MixNetwork.MixNetworkName) return true;
                __result = MixNetwork.s_enabledTags.ContainsKey(segment.type) ? GetEventClips(__instance, segment, metatag, newestFirst, flush) : [];
                return false;
            }
        }

        static List<AudioAsset> GetEventClips(Radio __instance, RuntimeSegment segment, AudioAsset.Metatag metatag, bool newestFirst = false, bool flush = false)
        {
            RadioTagSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<RadioTagSystem>();
            PrefabSystem orCreateSystemManaged = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<PrefabSystem>();
            List<AudioAsset> list = [];
            List<AudioAsset> list2 = [];
            while (list.Count < segment.clipsCap && existingSystemManaged.TryPopEvent(segment.type, newestFirst, out RadioTag radioTag))
            {
                list2.Clear();
                bool check(AudioAsset asset) => asset.ContainsTag(CustomRadios.FormatTagSegmentType(segment.type)) && MixNetwork.s_enabledTags[segment.type].Any(new Func<string, bool>(asset.ContainsTag));
                IEnumerable<AudioAsset> audioAssetList = AssetDatabase.global.GetAssets<AudioAsset>(SearchFilter<AudioAsset>.ByCondition(check));
                segment.clipsCap = audioAssetList.Count() > 1 ? 1 : audioAssetList.Count();
                foreach (AudioAsset audioAsset in audioAssetList)
                {
                    if (audioAsset.GetMetaTag(metatag) == orCreateSystemManaged.GetPrefab<PrefabBase>(radioTag.m_Event).name)
                    {
                        list2.Add(audioAsset);
                    }
                }
                if (list2.Count > 0)
                {
                    list.Add(list2[new Unity.Mathematics.Random((uint)DateTime.Now.Ticks).NextInt(0, list2.Count)]);
                }
            }
            if (flush)
            {
                existingSystemManaged.FlushEvents(segment.type);
            }
            return list;
        }
    }
}
