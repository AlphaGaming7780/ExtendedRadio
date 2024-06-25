using System.Drawing;
using System.IO;
using System.Linq;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Environment;
using Colossal.UI;
using ExtendedRadio.Systems;
using ExtendedRadio.UI;
using Game;
using Game.Modding;
using Game.SceneFlow;
using HarmonyLib;

namespace ExtendedRadio
{
	public class ExtendedRadioMod : IMod
	{
		public static ILog log = LogManager.GetLogger($"{nameof(ExtendedRadio)}").SetShowsErrorsInUI(false);

		internal static Setting _setting;
		internal static string modPath;
		static internal string resources; // = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "resources");
		// public static readonly string CustomRadiosPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CustomRadios");
		public static string PathToParent; //= Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName;
		internal static string PathToSettings;
		public static string PathToData;
		public static string ModsFolderCustomRadio;
		public static string ModsFolderRadioAddons;
		public static string PathToPDXMods;

		private Harmony harmony;

		public void OnLoad(UpdateSystem updateSystem)
		{
			log.Info(nameof(OnLoad));

            if (!GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset)) {log.Error("Failed to get the ExecutableAsset."); return;}

			_setting = new Setting(this);
			_setting.RegisterKeyBindings();
            _setting.RegisterInOptionsUI();
            AssetDatabase.global.LoadSettings("settings", _setting, new Setting(this));
            
			Localization.LoadLocalization();

			log.Info($"Current mod asset at {asset.path}");
			FileInfo fileInfo = new(asset.path);
			modPath = fileInfo.Directory.FullName;
			resources = Path.Combine(modPath, "resources");
			PathToParent = fileInfo.Directory.Parent.FullName;
			PathToSettings = Path.Combine(EnvPath.kUserDataPath, "ModsSettings", "ExtendedRadio");
			PathToData = Path.Combine(EnvPath.kUserDataPath, "ModsData", "ExtendedRadio");
			ModsFolderCustomRadio = Path.Combine(PathToData,"CustomRadios");
			ModsFolderRadioAddons = Path.Combine(PathToData,"RadioAddons");
			PathToPDXMods = Path.Combine(EnvPath.kUserDataPath, ".cache", "Mods", "mods_subscribed");

            if (Directory.Exists(ModsFolderCustomRadio)) CustomRadios.RegisterCustomRadioDirectory(ModsFolderCustomRadio);
            if (Directory.Exists(ModsFolderRadioAddons)) RadioAddons.RegisterRadioAddonsDirectory(ModsFolderRadioAddons);

            Icons.AddNewIconsFolder(modPath);
            Icons.LoadIconsFolder();

            updateSystem.UpdateAt<MainSystem>(SystemUpdatePhase.LateUpdate);
			updateSystem.UpdateAt<MixNetwork>(SystemUpdatePhase.UIUpdate);

			harmony = new($"{nameof(ExtendedRadio)}.{nameof(ExtendedRadioMod)}");
			harmony.PatchAll(typeof(ExtendedRadioMod).Assembly);
			var patchedMethods = harmony.GetPatchedMethods().ToArray();
			log.Info($"Plugin ExtendedRadio made patches! Patched methods: " + patchedMethods.Length);
			foreach (var patchedMethod in patchedMethods) log.Info($"Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}");
        }

		public void OnDispose()
		{
			log.Info(nameof(OnDispose));
			harmony.UnpatchAll($"{nameof(ExtendedRadio)}.{nameof(ExtendedRadioMod)}");
			UIManager.defaultUISystem.RemoveHostLocation(Icons.kIconsResourceKey, modPath);
			Icons.UnLoadIconsFolder();
            if (Directory.Exists(Path.Combine(EnvPath.kUserDataPath, "ModSettings", "ExtendedRadio"))) Directory.Delete(Path.Combine(EnvPath.kUserDataPath, "ModSettings", "ExtendedRadio"), true);
        }
	}
}
