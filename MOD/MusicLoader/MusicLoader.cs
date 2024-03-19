using System.Collections.Generic;
using System.IO;
using System.Linq;
using ATL;
using Colossal.IO.AssetDatabase;
using Colossal.Json;
using HarmonyLib;
using UnityEngine;
using static Colossal.IO.AssetDatabase.AudioAsset;
using static Game.Audio.Radio.Radio;

namespace ExtendedRadio
{
	public class MusicLoader
	{   
		public static AudioAsset LoadAudioFile(string audioFilePath, SegmentType segmentType, string networkName = null, string radioChannelName = null) {

			JsonAudioAsset jsAudioAsset;

			FileInfo fileInfo = new(audioFilePath);
			string JsonFile = $"{fileInfo.DirectoryName}\\{Path.GetFileNameWithoutExtension(fileInfo.FullName)}.json";

			if(File.Exists(JsonFile)) { //audioFilePath[..^".ogg".Count()]+".json"
				jsAudioAsset = Decoder.Decode(File.ReadAllText(JsonFile)).Make<JsonAudioAsset>();
			} else {
				jsAudioAsset = new();
			}


			AudioAsset audioAsset = new();
			audioAsset.AddTag($"AudioFilePath={audioFilePath}");
			audioAsset.AddTag($"AudioFileFormat={jsAudioAsset.AudioFileFormat.ToUpper()}");

			Dictionary<Metatag, string> m_Metatags = [];
			Traverse audioAssetTravers = Traverse.Create(audioAsset);

			Track track = new(audioFilePath, true);

			AddMetaTag(audioAsset, m_Metatags, Metatag.Title, jsAudioAsset.Title ?? track.Title);
			AddMetaTag(audioAsset, m_Metatags, Metatag.Album, jsAudioAsset.Album ?? track.Album);
			AddMetaTag(audioAsset, m_Metatags, Metatag.Artist, jsAudioAsset.Artist ?? track.Artist);
			AddMetaTag(audioAsset, m_Metatags, Metatag.Type, track, "TYPE", jsAudioAsset.Type ?? (segmentType.ToString() == "Playlist" ? "Music" : segmentType.ToString()));
			AddMetaTag(audioAsset, m_Metatags, Metatag.Brand, track, "BRAND", jsAudioAsset.Brand); // ?? "Brand"
			AddMetaTag(audioAsset, m_Metatags, Metatag.RadioStation, track, "RADIO STATION", networkName ?? jsAudioAsset.RadioStation );
			AddMetaTag(audioAsset, m_Metatags, Metatag.RadioChannel, track, "RADIO CHANNEL", radioChannelName ?? jsAudioAsset.RadioChannel );
			AddMetaTag(audioAsset, m_Metatags, Metatag.PSAType, track, "PSA TYPE", jsAudioAsset.PSAType);
			AddMetaTag(audioAsset, m_Metatags, Metatag.AlertType, track, "ALERT TYPE", jsAudioAsset.AlertType);
			AddMetaTag(audioAsset, m_Metatags, Metatag.NewsType, track, "NEWS TYPE", jsAudioAsset.NewsType);
			AddMetaTag(audioAsset, m_Metatags, Metatag.WeatherType, track, "WEATHER TYPE", jsAudioAsset.WeatherType);

			audioAssetTravers.Field("m_Metatags").SetValue(m_Metatags);
			audioAssetTravers.Field("durationMs").SetValue(track.DurationMs);
			audioAssetTravers.Field("m_Instance").SetValue(null);

			if (jsAudioAsset.loopStart == -1 && GetTimeTag(track, "LOOPSTART", out double time))
			{
				audioAssetTravers.Field("loopStart").SetValue(time);
			} else {
				audioAssetTravers.Field("loopStart").SetValue(jsAudioAsset.loopStart);
			}

			if (jsAudioAsset.loopEnd == -1 && GetTimeTag(track, "LOOPEND", out time))
			{
				audioAssetTravers.Field("loopEnd").SetValue(time);
			} else {
				audioAssetTravers.Field("loopEnd").SetValue(jsAudioAsset.loopEnd);
			}

			if (jsAudioAsset.alternativeStart == -1 && GetTimeTag(track, "ALTERNATIVESTART", out time))
			{
				audioAssetTravers.Field("alternativeStart").SetValue(time);
			} else {
				audioAssetTravers.Field("alternativeStart").SetValue(jsAudioAsset.alternativeStart);
			}

			if (jsAudioAsset.fadeoutTime == -1 && GetTimeTag(track, "FADEOUTTIME", out float time2))
			{
				audioAssetTravers.Field("fadeoutTime").SetValue(time2);
			} else {
				audioAssetTravers.Field("fadeoutTime").SetValue(jsAudioAsset.fadeoutTime);
			}

			return audioAsset;
		}

		internal static void AddMetaTag(AudioAsset audioAsset, Dictionary<Metatag, string> m_Metatags, Metatag tag, string value)
		{
			audioAsset.AddTag(value);
			m_Metatags[tag] = value;
		}

		internal static void AddMetaTag(AudioAsset audioAsset, Dictionary<Metatag, string> m_Metatags, Metatag tag, Track trackMeta, string oggTag, string value = null)
		{
			string extendedTag = value ?? GetExtendedTag(trackMeta, oggTag);
			if (!string.IsNullOrEmpty(extendedTag))
			{
				audioAsset.AddTag(oggTag.ToLower() + ":" + extendedTag);
				AddMetaTag(audioAsset, m_Metatags, tag, extendedTag);
			}
		}

		private static string GetExtendedTag(Track trackMeta, string tag)
		{
			if (trackMeta.AdditionalFields.TryGetValue(tag, out var value))
			{
				return value;
			}

			return null;
		}

		private static bool GetTimeTag(Track trackMeta, string tag, out double time)
		{
			if (trackMeta.AdditionalFields.TryGetValue(tag, out var value) && double.TryParse(value, out time))
			{
				return true;
			}

			time = -1.0;
			return false;
		}

		private static bool GetTimeTag(Track trackMeta, string tag, out float time)
		{
			if (trackMeta.AdditionalFields.TryGetValue(tag, out var value) && float.TryParse(value, out time))
			{
				return true;
			}

			time = -1f;
			return false;
		}

		internal static string GetClipPathFromAudiAsset(AudioAsset audioAsset) {

			foreach(string s in audioAsset.tags) {
				if(s.Contains("AudioFilePath=")) {
					// return s["AudioFilePath=".Length..];
					return s.Remove(0, "AudioFilePath=".Length);
				}
			}
			return "";
		}

		internal static AudioType GetClipFormatFromAudiAsset(AudioAsset audioAsset) {

			foreach(string s in audioAsset.tags) {
				if(s.Contains("AudioFileFormat=")) {

					return s.Remove(0, "AudioFileFormat=".Length) switch //s["AudioFileFormat=".Length..]
					{
						"ACC" => AudioType.ACC,
						"AIFF" => AudioType.AIFF,
						"IT" => AudioType.IT,
						"MOD" => AudioType.MOD,
						"MPEG" => AudioType.MPEG,
						"S3M" => AudioType.S3M,
						"WAV" => AudioType.WAV,
						"XM" => AudioType.XM,
						"XMA" => AudioType.XMA,
						"VAG" => AudioType.VAG,
						"AUDIOQUEUE" => AudioType.AUDIOQUEUE,
						_ => AudioType.OGGVORBIS,
					};
				}
			}
			return AudioType.OGGVORBIS;
		}
    }
}
