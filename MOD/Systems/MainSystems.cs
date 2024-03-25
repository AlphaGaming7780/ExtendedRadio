
using Colossal.Serialization.Entities;
using Game;
using System.Collections.Generic;
using UnityEngine;
using Colossal.PSI.Environment;
using System.IO;
using Game.UI.Menu;
using ExtendedRadio.UI;

namespace ExtendedRadio.Systems;

public partial class MainSystem : GameSystemBase
{
	//private bool canLoad = true;

	private readonly GameObject gameObject = new();
	internal static ExtendedRadioMono extendedRadioMono;
	internal static NotificationUISystem m_NotificationUISystem;

	protected override void OnCreate()
	{
		base.OnCreate();
		Enabled = false;
		extendedRadioMono = gameObject.AddComponent<ExtendedRadioMono>();
		m_NotificationUISystem = base.World.GetOrCreateSystemManaged<NotificationUISystem>();
	}

	protected override void OnUpdate() { }

	// NO USEFUL ANY MORE.
	//protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
	//{
	//	base.OnGameLoadingComplete(purpose, mode);
	//	// Print.Info($"OnGameLoadingComplete {purpose} | {mode}");

	//	if(mode == GameMode.MainMenu) {
	//		if(canLoad) {
	//			canLoad = false;

	//			//List<string> ModsFolderPaths = [];
	//			// if(Directory.Exists(EnvPath.kLocalModsPath)) ModsFolderPaths.Add(EnvPath.kLocalModsPath);
	//			// if(Directory.Exists(Mod.PathToPDXMods)) ModsFolderPaths.Add(Mod.PathToPDXMods);
	//			//if(Directory.Exists(Mod.ModsFolderCustomRadio)) CustomRadios.RegisterCustomRadioDirectory(Mod.ModsFolderCustomRadio);

	//			//extendedRadioMono.StartCoroutine(CustomRadios.SearchForCustomRadiosFolder(ModsFolderPaths));

	//               //Icons.LoadIconsFolder();

	//           }
	//	}
	//}
}