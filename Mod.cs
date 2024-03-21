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
	public class Mod : IMod
	{
		public static ILog log = LogManager.GetLogger($"{nameof(ExtendedRadio)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

		internal static Setting m_Setting;
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

			m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

            AssetDatabase.global.LoadSettings("settings", m_Setting, new Setting(this));

			// AssetDatabase.global.SaveSettings();

			log.Info($"Current mod asset at {asset.path}");
			FileInfo fileInfo = new(asset.path);
			modPath = fileInfo.Directory.FullName;
			resources = Path.Combine(modPath, "resources");
			PathToParent = fileInfo.Directory.Parent.FullName;
			PathToSettings = Path.Combine(EnvPath.kUserDataPath, "ModSettings", "ExtendedRadio");
			PathToData = Path.Combine(EnvPath.kUserDataPath, "ModsData", "ExtendedRadio");
			ModsFolderCustomRadio = Path.Combine(PathToData,"CustomRadios");
			ModsFolderRadioAddons = Path.Combine(PathToData,"RadioAddons");
			PathToPDXMods = Path.Combine(EnvPath.kUserDataPath, ".cache", "Mods", "mods_subscribed");

			// if(!Directory.Exists(resources)) {
			// 	Directory.CreateDirectory(resources);
			// 	File.Move(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DefaultIcon.svg"), Path.Combine(resources , "DefaultIcon.svg"));
			// }

			if(Directory.Exists(ModsFolderRadioAddons)) {
				RadioAddons.RegisterRadioAddonsDirectory(ModsFolderRadioAddons);
			}

			updateSystem.UpdateAt<MainSystem>(SystemUpdatePhase.LateUpdate);
			updateSystem.UpdateAt<ExtendedRadioUI>(SystemUpdatePhase.UIUpdate);

			harmony = new($"{nameof(ExtendedRadio)}.{nameof(Mod)}");
			harmony.PatchAll(typeof(Mod).Assembly);
			var patchedMethods = harmony.GetPatchedMethods().ToArray();
			log.Info($"Plugin ExtraDetailingTools made patches! Patched methods: " + patchedMethods.Length);
			foreach (var patchedMethod in patchedMethods)
			{
				log.Info($"Patched method: {patchedMethod.Module.Name}:{patchedMethod.Name}");
			}

			UIManager.defaultUISystem.AddHostLocation(Icons.IconsResourceKey, modPath);

		}

		public void OnDispose()
		{
			log.Info(nameof(OnDispose));
			harmony.UnpatchAll($"{nameof(ExtendedRadio)}.{nameof(Mod)}");
			UIManager.defaultUISystem.RemoveHostLocation(Icons.IconsResourceKey, modPath);
			Icons.UnLoadIconsFolder();
		}
	}

    internal class ExtraLibUI
    {
    }
}
