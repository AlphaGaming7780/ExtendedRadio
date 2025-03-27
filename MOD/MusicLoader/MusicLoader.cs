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
		public static AudioAsset[] LoadAudioFiles(string directory, SegmentType segmentType, string programName, string radioChannelName, string radioNetworkName)
		{
			AudioAsset[] audioAssets = [];
            DefaultAssetFactory.instance.GetAssetMimes(typeof(AudioAsset), out IReadOnlyList<string> formats);
            foreach (string format in formats) {
                foreach (string audioAssetFile in Directory.GetFiles(directory, $"*{format}"))
                {
                    AudioAsset audioAsset = LoadAudioFile(audioAssetFile, segmentType, programName, radioChannelName, radioNetworkName);
                    if(audioAsset != null) audioAssets = audioAssets.AddToArray(audioAsset);
                }
            }

			return audioAssets;
        }

		public static AudioAsset LoadAudioFile(string audioFilePath, SegmentType segmentType, string programName, string radioChannelName, string networkName) {

            audioFilePath = audioFilePath.Replace("\\", "/");

			JsonAudioAsset jsAudioAsset;

            //FileInfo fileInfo = new(audioFilePath);
            //string ogFileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            //string newFileName = ogFileName;
            //         string JsonFile = $"{fileInfo.DirectoryName}\\{ogFileName}.json";
            //         int x = 0;




            //while (AssetDatabase.global.TryGetAsset(SearchFilter<AudioAsset>.ByCondition((e) => { return e.name == newFileName; }), out AudioAsset audioAsset2))
            //{
            //	newFileName = ogFileName + $"_{x}";
            //	x++;
            //}

            //if(newFileName != ogFileName)
            //{
            //	string newaudioFilePath = $"{fileInfo.DirectoryName}\\{newFileName}{fileInfo.Extension}";
            //	Debug.Log(audioFilePath + " => " + newaudioFilePath);
            //             //fileInfo.MoveTo(audioFilePath);
            //             //File.Move(JsonFile, $"{fileInfo.DirectoryName}\\{newFileName}.json");
            //	//JsonFile = $"{fileInfo.DirectoryName}\\{newFileName}.json";
            //         }


            AudioAsset audioAsset = LoadAudioAsset(audioFilePath, segmentType, out string jsonFilePath);

            if (audioAsset == null) return null;

            if (File.Exists(jsonFilePath)) {
				jsAudioAsset = Decoder.Decode(File.ReadAllText(jsonFilePath)).Make<JsonAudioAsset>();
			} else {
				jsAudioAsset = new();
			}

			

    //        AudioAsset audioAsset;
    //        AssetDataPath assetDataPath = AssetDataPath.Create(audioFilePath, EscapeStrategy.None);
    //        try
    //        {
    //            IAssetData assetData = AssetDatabase.game.AddAsset(assetDataPath);
				//if(assetData is AudioAsset audioAsset1)
				//{
				//	audioAsset = audioAsset1;
    //            }
				//else
				//{
				//	return null;
				//}

    //        }
    //        catch (Exception e)
    //        {
    //            ExtendedRadioMod.log.Warn(e);
				//return null;
    //        }

            //Track track = new(audioFilePath, true);

            using (Stream writeStream = audioAsset.GetReadStream())
            {
                Dictionary<Metatag, string> m_Metatags = [];
                Traverse audioAssetTravers = Traverse.Create(audioAsset);
                Track track = new(writeStream, audioAsset.database.GetMeta(audioAsset.id.guid).mimeType, null);
                AddMetaTag(audioAsset, m_Metatags, Metatag.Title, string.IsNullOrEmpty(jsAudioAsset.Title) ? track.Title : jsAudioAsset.Title);
                AddMetaTag(audioAsset, m_Metatags, Metatag.Album, string.IsNullOrEmpty(jsAudioAsset.Album) ? track.Album : jsAudioAsset.Album);
                AddMetaTag(audioAsset, m_Metatags, Metatag.Artist, string.IsNullOrEmpty(jsAudioAsset.Artist) ? track.Artist : jsAudioAsset.Artist);
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
			audioAsset.AddTags(CustomRadios.FormatTags(segmentType, programName, radioChannelName, networkName));

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
			string extendedTag = string.IsNullOrEmpty(value) ? GetExtendedTag(trackMeta, oggTag) : value;
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

		private static AudioAsset LoadAudioAsset(string audioFilePath, SegmentType segmentType, out string jsonFilePath)
		{
            FileInfo fileInfo = new(audioFilePath);
            string newAudioFilePath = audioFilePath;
            string ogFileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            string newFileName = ogFileName;
            jsonFilePath = $"{fileInfo.DirectoryName}\\{ogFileName}.json";
            int x = 0;

            if (segmentType == SegmentType.Commercial)
            {
                while (AssetDatabase.global.TryGetAsset<AudioAsset>(SearchFilter<AudioAsset>.ByCondition((audioAsset) => { return audioAsset.path != audioFilePath && audioAsset.name == newFileName; }), out AudioAsset a))
                {
                    newFileName = ogFileName + $"_{x}";
                    x++;
                }

                newAudioFilePath = $"{fileInfo.DirectoryName}\\{newFileName}{fileInfo.Extension}";

                if (ogFileName != newFileName)
                {;
                    fileInfo.MoveTo(newAudioFilePath);
                    if (File.Exists(jsonFilePath))
                    {
                        File.Move(jsonFilePath, $"{fileInfo.DirectoryName}\\{newFileName}.json");
                    }
                    jsonFilePath = $"{fileInfo.DirectoryName}\\{newFileName}.json";
                }

            }

            if (AssetDatabase.global.TryGetAsset<AudioAsset>(
				SearchFilter<AudioAsset>.ByCondition( 
					(audioAsset) => 
					{
						return audioAsset.name == ogFileName && audioAsset.path == audioFilePath;
					}
				), 
				out AudioAsset audioAsset)
			)
			{
				if(ogFileName != newFileName)
				{
                    audioAsset.Delete();
                    audioAsset = LoadAudioAssetFromFile(newAudioFilePath);
                }
            } else
			{
				audioAsset = LoadAudioAssetFromFile(audioFilePath);
            }

			return audioAsset;
		}

		private static AudioAsset LoadAudioAssetFromFile(string audioFilePath)
		{
            FileInfo fileInfo = new(audioFilePath);
            AssetDataPath assetDataPath = AssetDataPath.Create(fileInfo.DirectoryName, fileInfo.Name, EscapeStrategy.None);
            try
            {
                IAssetData assetData = AssetDatabase.user.AddAsset(assetDataPath);
                if (assetData is AudioAsset audioAsset1)
                {
                    return audioAsset1;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                ExtendedRadioMod.log.Warn(e);
                return null;
            }
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

		internal static AudioType GetClipFormatFromFileExtension(string fileExtension)
		{
            // https://docs.unity3d.com/ScriptReference/AudioType.html
			fileExtension = fileExtension.TrimStart('.');
            return fileExtension.ToUpper() switch
			{
				"OGG" => AudioType.OGGVORBIS,
				"OGA" => AudioType.OGGVORBIS,
                "ACC" => AudioType.ACC, //NOT SUPPORTED
				"MP4" => AudioType.ACC, //NOT SUPPORTED
				"M4A" => AudioType.ACC, //NOT SUPPORTED
                "AIFF" => AudioType.AIFF, //WAV BUT FOR APPLE PC
				"IT" => AudioType.IT, //IDK
				"MOD" => AudioType.MOD,
				"MP3" => AudioType.MPEG,
				"S3M" => AudioType.S3M,
				"WAV" => AudioType.WAV,
				"XM" => AudioType.XM,
				"XMA" => AudioType.XMA, //XBOX 360
				"VAG" => AudioType.VAG, // PLAYSTATION
				"AUDIOQUEUE" => AudioType.AUDIOQUEUE,
				_ => AudioType.UNKNOWN,
			};
		}
	}
}
