using System;
using System.Collections.Generic;
using System.IO;
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
		public static AudioAsset LoadAudioFile(string audioFilePath, SegmentType segmentType, string programName, string networkName, string radioChannelName) {

			JsonAudioAsset jsAudioAsset;

			FileInfo fileInfo = new(audioFilePath);
			string JsonFile = $"{fileInfo.DirectoryName}\\{Path.GetFileNameWithoutExtension(fileInfo.FullName)}.json";

			if(File.Exists(JsonFile)) {
				jsAudioAsset = Decoder.Decode(File.ReadAllText(JsonFile)).Make<JsonAudioAsset>();
			} else {
				jsAudioAsset = new();
			}

            AudioAsset audioAsset = new();
            AssetDataPath assetDataPath = AssetDataPath.Create(audioFilePath, EscapeStrategy.None);
            try
            {
                IAssetData assetData = AssetDatabase.game.AddAsset(assetDataPath);
				if(assetData is AudioAsset audioAsset1)
				{
					audioAsset = audioAsset1;
                }
				else
				{
					return null;
				}

            }
            catch (Exception e)
            {
                Mod.log.Warn(e);
				return null;
            }

            //Track track = new(audioFilePath, true);

            using (Stream writeStream = audioAsset.GetReadStream())
            {
                Dictionary<Metatag, string> m_Metatags = [];
                Traverse audioAssetTravers = Traverse.Create(audioAsset);
                Track track = new(writeStream, audioAsset.database.GetMeta(audioAsset.guid).mimeType, null);
                AddMetaTag(audioAsset, m_Metatags, Metatag.Title, jsAudioAsset.Title ?? track.Title);
                AddMetaTag(audioAsset, m_Metatags, Metatag.Album, jsAudioAsset.Album ?? track.Album);
                AddMetaTag(audioAsset, m_Metatags, Metatag.Artist, jsAudioAsset.Artist ?? track.Artist);
                AddMetaTag(audioAsset, m_Metatags, Metatag.Type, track, "TYPE", jsAudioAsset.Type ?? (segmentType.ToString() == "Playlist" ? "Music" : segmentType.ToString()));
                AddMetaTag(audioAsset, m_Metatags, Metatag.Brand, track, "BRAND", jsAudioAsset.Brand);
                AddMetaTag(audioAsset, m_Metatags, Metatag.RadioStation, track, "RADIO STATION", networkName);
                AddMetaTag(audioAsset, m_Metatags, Metatag.RadioChannel, track, "RADIO CHANNEL", radioChannelName);
                AddMetaTag(audioAsset, m_Metatags, Metatag.PSAType, track, "PSA TYPE", jsAudioAsset.PSAType);
                AddMetaTag(audioAsset, m_Metatags, Metatag.AlertType, track, "ALERT TYPE", jsAudioAsset.AlertType);
                AddMetaTag(audioAsset, m_Metatags, Metatag.NewsType, track, "NEWS TYPE", jsAudioAsset.NewsType);
                AddMetaTag(audioAsset, m_Metatags, Metatag.WeatherType, track, "WEATHER TYPE", jsAudioAsset.WeatherType);

                audioAssetTravers.Field("m_Metatags").SetValue(m_Metatags);

                //bool isDirty = false;
                
                //if (track.Title != jsAudioAsset.Title) { track.Title = jsAudioAsset.Title; isDirty = true; }
                //if (track.Album != jsAudioAsset.Album) { track.Album = jsAudioAsset.Album; isDirty = true; }
                //if (track.Artist != jsAudioAsset.Artist) { track.Artist = jsAudioAsset.Artist; isDirty = true; }

                //isDirty |= SetAdditionalField(track, "TYPE", jsAudioAsset.Type);
                //isDirty |= SetAdditionalField(track, "BRAND", jsAudioAsset.Brand);
                //isDirty |= SetAdditionalField(track, "RADIO STATION", networkName);
                //isDirty |= SetAdditionalField(track, "RADIO CHANNEL", radioChannelName);
                //isDirty |= SetAdditionalField(track, "PSA TYPE", jsAudioAsset.PSAType);
                //isDirty |= SetAdditionalField(track, "ALERT TYPE", jsAudioAsset.AlertType);
                //isDirty |= SetAdditionalField(track, "NEWS TYPE", jsAudioAsset.NewsType);
                //isDirty |= SetAdditionalField(track, "WEATHER TYPE", jsAudioAsset.WeatherType);

                //if (isDirty) track.Save();
            }


            audioAsset.AddTags(jsAudioAsset.tags);
			audioAsset.AddTags([segmentType.ToString(), programName, radioChannelName, networkName]);

			return audioAsset;
		}

		private static bool SetAdditionalField(Track track, string field, string value)
		{
			if (track.AdditionalFields == null) Debug.Log("MDR");
            if (track.AdditionalFields.TryGetValue(field, out string outValue)) 
			{ 
				if (outValue != value) 
				{ 
					track.AdditionalFields[field] = value; 
					return true; 
				} 
			}
            else { track.AdditionalFields.Add(field, value); return true; }
			return false;
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

		//internal static string GetClipPathFromAudiAsset(AudioAsset audioAsset) {

		//	foreach(string s in audioAsset.tags) {
		//		if(s.Contains("AudioFilePath=")) {
		//			// return s["AudioFilePath=".Length..];
		//			return s.Remove(0, "AudioFilePath=".Length);
		//		}
		//	}
		//	return "";
		//}

		//internal static AudioType GetClipFormatFromAudiAsset(AudioAsset audioAsset) {

		//	foreach(string s in audioAsset.tags) {
		//		if(s.Contains("AudioFileFormat=")) {

		//			return s.Remove(0, "AudioFileFormat=".Length) switch //s["AudioFileFormat=".Length..]
		//			{
		//				"ACC" => AudioType.ACC,
		//				"AIFF" => AudioType.AIFF,
		//				"IT" => AudioType.IT,
		//				"MOD" => AudioType.MOD,
		//				"MPEG" => AudioType.MPEG,
		//				"S3M" => AudioType.S3M,
		//				"WAV" => AudioType.WAV,
		//				"XM" => AudioType.XM,
		//				"XMA" => AudioType.XMA,
		//				"VAG" => AudioType.VAG,
		//				"AUDIOQUEUE" => AudioType.AUDIOQUEUE,
		//				_ => AudioType.OGGVORBIS,
		//			};
		//		}
		//	}
		//	return AudioType.OGGVORBIS;
		//}
    }
}
