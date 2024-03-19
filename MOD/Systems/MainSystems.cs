
using Colossal.Serialization.Entities;
using Game;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Collections;
using Game.UI.Menu;
using Game.Rendering;
using Game.Tools;
using Game.UI.InGame;
using Game.Prefabs;
using Colossal.PSI.Common;
using System;
using System.Threading;
using System.Collections;
using UnityEngine;
using Colossal.PSI.Environment;

namespace ExtendedRadio.Systems;

public partial class MainSystem : GameSystemBase
{
	private bool canLoad = true;

    private GameObject gameObject = new();
    internal static ExtendedRadioMono extendedRadioMono;
 
	protected override void OnCreate()
	{
		base.OnCreate();
		Enabled = false;
        extendedRadioMono = gameObject.AddComponent<ExtendedRadioMono>();
	}

	protected override void OnUpdate() {}

	protected override void OnGamePreload(Purpose purpose, GameMode mode)
	{
		base.OnGamePreload(purpose, mode);
		// Print.Info($"OnGamePreload {purpose} | {mode}");
	}

	protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
	{
		base.OnGameLoadingComplete(purpose, mode);
		// Print.Info($"OnGameLoadingComplete {purpose} | {mode}");

		if(mode == GameMode.MainMenu) {
			if(canLoad) {
				canLoad = false;
				// UnityEngine.Debug.Log(EnvPath.kLocalModsPath);
				// UnityEngine.Debug.Log(EnvPath.kUserDataPath);

				// extendedRadioMono.StartCoroutine(CustomRadios.SearchForCustomRadiosFolder(Mod.PathToParent));
				extendedRadioMono.StartCoroutine(CustomRadios.SearchForCustomRadiosFolder([EnvPath.kLocalModsPath, Mod.PathToPDXMods]));
				// extendedRadioMono.StartCoroutine(RadioAddons.([EnvPath.kLocalModsPath, Mod.PathToPDXMods]));
				// extendedRadioMono.StartCoroutine(CustomRadios.SearchForCustomRadiosFolder(EnvPath.k));



			}
		}
	}

}