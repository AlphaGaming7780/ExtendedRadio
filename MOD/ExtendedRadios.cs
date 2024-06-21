using System.Collections.Generic;
using System.Reflection;
using Colossal.IO.AssetDatabase;
using Game.Audio.Radio;
using HarmonyLib;
using static Game.Audio.Radio.Radio;

namespace ExtendedRadio
{
	public static class ExtendedRadio
	{
		public delegate void onRadioLoaded();
		public delegate void onRadioPaused();
		public delegate void onRadioUnPaused();
		public delegate void onRadioNextSong();
		public delegate void onRadioPreviousSong();
		public delegate void onRadioVolumeChanged(float volume);
		public delegate void onRadioStationChanged(string name);
        public static event onRadioLoaded OnRadioLoaded;
        public static event onRadioPaused OnRadioPaused;
        public static event onRadioUnPaused OnRadioUnPaused;
        public static event onRadioNextSong OnRadioNextSong;
        public static event onRadioPreviousSong OnRadioPreviousSong;
        public static event onRadioVolumeChanged OnRadioVolumeChanged;
        public static event onRadioStationChanged OnRadioStationChanged;
        internal static readonly Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<SegmentType, List<AudioAsset>>>>> audioDataBase = [];
		public static Traverse radioTravers;
		public static Radio radio;

		static internal void OnLoadRadio(Radio __instance) { 

			audioDataBase.Clear();

			radio = __instance;
			radioTravers = Traverse.Create(__instance);

			CustomRadios.LoadCustomRadios();
			RadioAddons.LoadRadioAddons();

			OnRadioLoaded?.Invoke();

        }

		internal static System.IO.Stream GetEmbedded(string embeddedPath) {
			return Assembly.GetExecutingAssembly().GetManifestResourceStream("ExtendedRadio.embedded."+embeddedPath);
        }

		internal static void RadioPaused()
		{
			OnRadioPaused?.Invoke();
		}

        internal static void RadioUnPaused()
        {
            OnRadioUnPaused?.Invoke();
        }

        internal static void RadioNextSong()
        {
            OnRadioNextSong?.Invoke();
        }

        internal static void RadioPreviousSong()
        {
            OnRadioPreviousSong?.Invoke();
        }

		internal static void RadioVolumeChanged(float volume)
		{
            OnRadioVolumeChanged?.Invoke(volume);
        }

        internal static void RadioStationChanged(string name)
        {
            OnRadioStationChanged?.Invoke(name);
        }

    }
}