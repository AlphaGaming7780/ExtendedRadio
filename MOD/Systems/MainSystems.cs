using Colossal.Serialization.Entities;
using Game;
using UnityEngine;
using Game.UI.InGame;
using HarmonyLib;
using Game.Input;

namespace ExtendedRadio.Systems;

public partial class MainSystem : GameSystemBase
{
    private ProxyAction _pauseRadio;
	private RadioUISystem _radioUISystem;
	private Traverse _radioUISystemTraverse;

    protected override void OnCreate()
	{
		base.OnCreate();
		_radioUISystem = World.GetOrCreateSystemManaged<RadioUISystem>();
        _pauseRadio = ExtendedRadioMod._setting.GetAction("PauseRadioBinding");

		_radioUISystemTraverse = Traverse.Create(_radioUISystem);

    }

    protected override void OnUpdate() {
		//if (_pauseRadio.WasPerformedThisFrame())
		//{
		//	Debug.Log("Pausing Radio");	
		//	_radioUISystemTraverse.Method("SetPaused").GetValue(!ExtendedRadio.radio.paused);
		//}
	}

	protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
	{
		if(purpose == Purpose.LoadGame && mode == GameMode.Game)
		{
            _radioUISystemTraverse.Method("SetSkipAds", [typeof(bool)])?.GetValue(ExtendedRadioMod._setting.DisableAdsOnStartup);
        }
	}
}