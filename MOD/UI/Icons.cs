using System.Collections;
using System.Collections.Generic;
using System.IO;
using Colossal.UI;

namespace ExtendedRadio.UI;

static class Icons
{
	private static readonly List<string> pathToIconToLoad = [];
	internal static readonly string IconsResourceKey = "extendedradio";
	public static readonly string COUIBaseLocation = $"coui://{IconsResourceKey}";
	private static bool iconLoaded = false;

	internal static void AddNewIconsFolder(string pathToFolder) {
		if(!pathToIconToLoad.Contains(pathToFolder)) pathToIconToLoad.Add(pathToFolder);
		if(iconLoaded) UIManager.defaultUISystem.AddHostLocation(IconsResourceKey, pathToFolder, false);
    }

	internal static void RemoveNewIconsFolder(string pathToFolder) {
		pathToIconToLoad.Remove(pathToFolder);
		if(iconLoaded) UIManager.defaultUISystem.RemoveHostLocation(IconsResourceKey, pathToFolder);
	}

	internal static void LoadIconsFolder() {
		iconLoaded = true;
		foreach(string path in pathToIconToLoad) {
			UIManager.defaultUISystem.AddHostLocation(IconsResourceKey, path, false);
		}
	}

	internal static void UnLoadIconsFolder() {
		iconLoaded = false;
		foreach(string path in pathToIconToLoad) {
			UIManager.defaultUISystem.RemoveHostLocation(IconsResourceKey, path);
		}
	}
}