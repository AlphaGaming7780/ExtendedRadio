using Colossal.Serialization.Entities;
using Game;
using UnityEngine;
using Game.UI.InGame;
using HarmonyLib;
using Game.Input;
using Game.Prefabs;
using Game.Settings;

namespace ExtendedRadio.Systems;

public partial class MainSystem : GameSystemBase
{
	private RadioUISystem _radioUISystem;
	private Traverse _radioUISystemTraverse;

    private ProxyAction _pauseRadioBinding;
    private ProxyAction _muteRadioBinding;
    private ProxyAction _nextSongRadioBinding;
    private ProxyAction _prevSongRadioBinding;
    private ProxyAction _volumeUpRadioBinding;
    private ProxyAction _volumeDownRadioBinding;

    protected override void OnCreate()
	{
		base.OnCreate();
		_radioUISystem = World.GetOrCreateSystemManaged<RadioUISystem>();
        _radioUISystemTraverse = Traverse.Create(_radioUISystem);

        _pauseRadioBinding = ExtendedRadioMod.s_setting.GetAction(nameof(ExtendedRadioMod.s_setting.PauseRadioBinding));
        _muteRadioBinding = ExtendedRadioMod.s_setting.GetAction(nameof(ExtendedRadioMod.s_setting.MuteRadioBinding));
        _nextSongRadioBinding = ExtendedRadioMod.s_setting.GetAction(nameof(ExtendedRadioMod.s_setting.NextSongRadioBinding));
        _prevSongRadioBinding = ExtendedRadioMod.s_setting.GetAction(nameof(ExtendedRadioMod.s_setting.PrevSongRadioBinding));
        _volumeUpRadioBinding = ExtendedRadioMod.s_setting.GetAction(nameof(ExtendedRadioMod.s_setting.VolumeUpRadioBinding));
        _volumeDownRadioBinding = ExtendedRadioMod.s_setting.GetAction(nameof(ExtendedRadioMod.s_setting.VolumeDownRadioBinding));
    }

    protected override void OnUpdate() {
		if (_pauseRadioBinding.WasPerformedThisFrame() && ExtendedRadio.radio != null && ExtendedRadio.radio.isEnabled) _radioUISystemTraverse.Method("SetPaused", [typeof(bool)]).GetValue(!ExtendedRadio.radio.paused);
        if (_muteRadioBinding.WasPerformedThisFrame() && ExtendedRadio.radio != null && ExtendedRadio.radio.isEnabled) _radioUISystemTraverse.Method("SetMuted", [typeof(bool)]).GetValue(!ExtendedRadio.radio.muted);
        if (_nextSongRadioBinding.WasPerformedThisFrame() && ExtendedRadio.radio != null && ExtendedRadio.radio.isEnabled) ExtendedRadio.radio.NextSong();
        if (_prevSongRadioBinding.WasPerformedThisFrame() && ExtendedRadio.radio != null && ExtendedRadio.radio.isEnabled) ExtendedRadio.radio.PreviousSong();
        if (_volumeUpRadioBinding.WasPerformedThisFrame() && ExtendedRadio.radio != null && ExtendedRadio.radio.isEnabled) _radioUISystemTraverse.Method("SetVolume", [typeof(float)]).GetValue(SharedSettings.instance.audio.radioVolume + 0.05f);
        if (_volumeDownRadioBinding.WasPerformedThisFrame() && ExtendedRadio.radio != null && ExtendedRadio.radio.isEnabled) _radioUISystemTraverse.Method("SetVolume", [typeof(float)]).GetValue(SharedSettings.instance.audio.radioVolume - 0.05f);
    }
}