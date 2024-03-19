using System.Collections.Generic;
using System.IO;
using Colossal.Json;

namespace ExtendedRadio
{
	public class RadioAddons
	{
		private static readonly List<string> addonsDirectories = [];
		internal static void LoadRadioAddons() {
			foreach(string radioAddonsFolder in addonsDirectories ) {
				foreach(string folder in Directory.GetDirectories(radioAddonsFolder)) {
					if(File.Exists(folder+"\\RadioAddon.json")) {
						RadioAddon radioAddons = Decoder.Decode(File.ReadAllText(folder+"\\RadioAddon.json")).Make<RadioAddon>();
						foreach(string audioFileFolder in Directory.GetDirectories(folder)) {
							foreach(string audioAssetFile in Directory.GetFiles(audioFileFolder, "*.ogg")) {
								ExtendedRadio.AddAudioToDataBase(radioAddons.RadioNetwork, radioAddons.RadioChannel, radioAddons.Program, CustomRadios.StringToSegmentType(radioAddons.SegmentType), MusicLoader.LoadAudioFile(audioAssetFile, CustomRadios.StringToSegmentType(radioAddons.SegmentType), radioAddons.RadioNetwork, radioAddons.RadioChannel));
							}
						}
					}
				}
			}
		}

		public static void RegisterRadioAddonsDirectory(string path) {
			addonsDirectories.Add(path);
		}
	}
}