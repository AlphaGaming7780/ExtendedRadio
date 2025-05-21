using System.Collections.Generic;
using Colossal.UI;

namespace ExtendedRadio.UI
{
    static class Icons
    {
        private static readonly List<string> _pathToIconToLoad = new();
        public const string kIconsResourceKey = "extendedradio";
        public static readonly string COUIBaseLocation = $"coui://{kIconsResourceKey}";
        public static readonly string DefaultRadioIcon = $"{COUIBaseLocation}/resources/DefaultIcon.svg";
        public static readonly string MixNetworkIcon = $"{COUIBaseLocation}/resources/MixNetwork.svg";
        private static bool _iconLoaded = false;

        internal static void AddNewIconsFolder(string pathToFolder)
        {
            if (!_pathToIconToLoad.Contains(pathToFolder)) _pathToIconToLoad.Add(pathToFolder);
            if (_iconLoaded) UIManager.defaultUISystem.AddHostLocation(kIconsResourceKey, pathToFolder, false);
        }

        internal static void RemoveNewIconsFolder(string pathToFolder)
        {
            _pathToIconToLoad.Remove(pathToFolder);
            if (_iconLoaded) UIManager.defaultUISystem.RemoveHostLocation(kIconsResourceKey, pathToFolder);
        }

        internal static void LoadIconsFolder()
        {
            _iconLoaded = true;
            foreach (string path in _pathToIconToLoad)
            {
                UIManager.defaultUISystem.AddHostLocation(kIconsResourceKey, path, false);
            }
        }

        internal static void UnLoadIconsFolder()
        {
            _iconLoaded = false;
            foreach (string path in _pathToIconToLoad)
            {
                UIManager.defaultUISystem.RemoveHostLocation(kIconsResourceKey, path);
            }
        }
    }
}