
using Colossal.Serialization.Entities;
using Game;
using System.Collections.Generic;
using UnityEngine;
using Colossal.PSI.Environment;
using System.IO;
using Game.UI.Menu;
using ExtendedRadio.UI;
using Game.Audio;
using Game.UI.InGame;
using HarmonyLib;
using Colossal.UI.Binding;

namespace ExtendedRadio.Systems;

public partial class MainSystem : GameSystemBase
{
	//internal static NotificationUISystem m_NotificationUISystem;

	protected override void OnCreate()
	{
		base.OnCreate();
		Enabled = false;
		//m_NotificationUISystem = base.World.GetOrCreateSystemManaged<NotificationUISystem>();
	}

	protected override void OnUpdate() { }

	protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
	{
		if(purpose == Purpose.LoadGame && mode == GameMode.Game)
		{
			Traverse.Create(base.World.GetExistingSystemManaged<RadioUISystem>())?.Method("SetSkipAds", [typeof(bool)])?.GetValue(Mod.m_Setting.DisableAdsOnStartup);
        }
	}
}