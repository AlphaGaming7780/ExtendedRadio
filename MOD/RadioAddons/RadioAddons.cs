using System.Collections.Generic;
using System.IO;
using Colossal.Json;

namespace ExtendedRadio
{
	public class RadioAddons
	{
		private static readonly List<string> addonsDirectories = new();
		internal static void LoadRadioAddons() {
			foreach(string radioAddonsFolder in addonsDirectories ) {
				foreach(string folder in Directory.GetDirectories(radioAddonsFolder)) {
					if(File.Exists(folder+"\\RadioAddon.json")) {
						RadioAddon radioAddons = Decoder.Decode(File.ReadAllText(folder+"\\RadioAddon.json")).Make<RadioAddon>();
						foreach(string audioFileFolder in Directory.GetDirectories(folder)) {
							MusicLoader.LoadAudioFiles(audioFileFolder, CustomRadios.StringToSegmentType(radioAddons.SegmentType), radioAddons.Program, radioAddons.RadioNetwork, radioAddons.RadioChannel);
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