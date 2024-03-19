using System.Collections.Generic;
using System.IO;
using Colossal.UI;

namespace ExtendedRadio.UI;

static class Icons
{
	private static readonly List<string> pathToIconToLoad = [Mod.modPath];
	static readonly string IconsResourceKey = "extendedradio"; //= $"{MyPluginInfo.PLUGIN_NAME.ToLower()}";
	public static readonly string COUIBaseLocation = $"coui://{IconsResourceKey}";

	internal static void AddNewIconsFolder(string pathToFolder) {
		if(!pathToIconToLoad.Contains(pathToFolder)) pathToIconToLoad.Add(pathToFolder);
	}

	internal static void LoadIconsFolder() {
		foreach(string path in pathToIconToLoad) {
			UIManager.defaultUISystem.AddHostLocation(IconsResourceKey, path);
		}
	}

	internal static void UnLoadIconsFolder() {
		foreach(string path in pathToIconToLoad) {
			UIManager.defaultUISystem.RemoveHostLocation(IconsResourceKey, path);
		}
	}
}