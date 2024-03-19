using System.Collections.Generic;
using System.Reflection;
using Colossal.IO.AssetDatabase;
using Game.Audio.Radio;
using HarmonyLib;
using static Game.Audio.Radio.Radio;

namespace ExtendedRadio
{
	public class ExtendedRadio
	{
		public delegate void OnRadioLoad();
		public static event OnRadioLoad CallOnRadioLoad;
		internal static readonly Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<SegmentType, List<AudioAsset>>>>> audioDataBase = [];
		public static Traverse radioTravers = null;
		public static Radio radio = null;
		static internal void OnLoadRadio(Radio __instance) { 

			audioDataBase.Clear();

			radio = __instance;
			radioTravers = Traverse.Create(__instance);

			CustomRadios.LoadCustomRadios();
			RadioAddons.LoadRadioAddons();

			try {
				CallOnRadioLoad();
			} catch {}
		}

		static internal void AddAudioToDataBase(RadioChannel radioChannel) {
			foreach(Program program in radioChannel.programs) {
				foreach(Segment segment in program.segments) {
					if(audioDataBase.ContainsKey(radioChannel.network)){
						if(audioDataBase[radioChannel.network].ContainsKey(radioChannel.name)) {
							if(audioDataBase[radioChannel.network][radioChannel.name].ContainsKey(program.name)) {
								if(audioDataBase[radioChannel.network][radioChannel.name][program.name].ContainsKey(segment.type)) {
                                    audioDataBase[radioChannel.network][radioChannel.name][program.name][segment.type].AddRange([..segment.clips]);
								} else {
									audioDataBase[radioChannel.network][radioChannel.name][program.name].Add(segment.type, [..segment.clips]);
								}
							} else {
								Dictionary<SegmentType, List<AudioAsset>> dict1 = [];
								dict1.Add(segment.type, [..segment.clips]);

								audioDataBase[radioChannel.network][radioChannel.name].Add(program.name, dict1);
							}
						} else {
							Dictionary<SegmentType, List<AudioAsset>> dict1 = [];
							dict1.Add(segment.type, [..segment.clips]);

							Dictionary<string, Dictionary<SegmentType, List<AudioAsset>>> dict2 = [];
							dict2.Add(program.name, dict1);

							audioDataBase[radioChannel.network].Add(radioChannel.name, dict2);
						}	
					} else {

						Dictionary<SegmentType, List<AudioAsset>> dict1 = [];
						dict1.Add(segment.type, [..segment.clips]);

						Dictionary<string, Dictionary<SegmentType, List<AudioAsset>>> dict2 = [];
						dict2.Add(program.name, dict1);

						Dictionary<string, Dictionary<string, Dictionary<SegmentType, List<AudioAsset>>>> dict3 = [];
						dict3.Add(radioChannel.name, dict2);
						audioDataBase.Add(radioChannel.network, dict3);
					}
				}
			}
		}

		static internal void AddAudioToDataBase(string network, string radioChannel, string program, SegmentType segmentType, List<AudioAsset> audioAssets) {
			audioDataBase[network][radioChannel][program][segmentType].AddRange(audioAssets);
		}

		static internal void AddAudioToDataBase(string network, string radioChannel, string program, SegmentType segmentType, AudioAsset audioAssets) {
			audioDataBase[network][radioChannel][program][segmentType].Add(audioAssets);
		}

		static internal List<AudioAsset> GetAudioAssetsFromAudioDataBase(Radio radio, SegmentType type) {

			return audioDataBase[radio.currentChannel.network][radio.currentChannel.name][radio.currentChannel.currentProgram.name][type];
		}

		internal static System.IO.Stream GetEmbedded(string embeddedPath) {
			return Assembly.GetExecutingAssembly().GetManifestResourceStream("ExtendedRadio.embedded."+embeddedPath);
        }
    }
}