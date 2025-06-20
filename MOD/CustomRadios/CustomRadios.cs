using System.IO;
using System.Collections.Generic;
using Colossal.Json;
using static Game.Audio.Radio.Radio;
using HarmonyLib;
using ExtendedRadio.UI;
using static Game.Rendering.Debug.RenderPrefabRenderer;

namespace ExtendedRadio
{

	/// <summary>This is the main class for the CustomRadios feature.</summary>
	public class CustomRadios
	{
		private static readonly List<string> radioDirectories = new();
		internal static List<string> customeRadioChannelsName = new();
		private static readonly List<string> customeNetworksName = new();
		private static Dictionary<string, RadioNetwork> m_Networks = new();
		private static Dictionary<string, RuntimeRadioChannel> m_RadioChannels = new();
		private static int radioNetworkIndex;
		static internal void LoadCustomRadios() {

			m_Networks = ExtendedRadio.radioTravers.Field("m_Networks").GetValue<Dictionary<string, RadioNetwork>>();
			m_RadioChannels = ExtendedRadio.radioTravers.Field("m_RadioChannels").GetValue<Dictionary<string, RuntimeRadioChannel>>();

			radioNetworkIndex = m_Networks.Count;

			foreach(string radioDirectory in radioDirectories) {
				foreach(string radioNetwork in Directory.GetDirectories( radioDirectory )) {
					if(radioNetwork != radioDirectory) {
						// if(Directory.GetFiles(radioNetwork, "*.ogg").Length == 0) {
							
						RadioNetwork network = new();

						if(File.Exists(radioNetwork + "//RadioNetwork.json")) {
							network = JsonToRadioNetwork(radioNetwork);
						} else {
							network.name = new DirectoryInfo(radioNetwork).Name;
							network.nameId = null;
							network.description = "A custom Network";
							network.descriptionId = null;
							network.icon = File.Exists(Path.Combine(radioNetwork, "icon.svg")) ? $"{Icons.COUIBaseLocation}/CustomRadios/{new DirectoryInfo(radioNetwork).Name}/icon.svg" : Icons.DefaultRadioIcon;
							network.allowAds = true;
						}
						network.icon ??= Icons.DefaultRadioIcon;

						network.uiPriority = radioNetworkIndex++;

						if(!m_Networks.ContainsKey(network.name)) {
							customeNetworksName.Add(network.name);
							m_Networks.Add(network.name, network);
						}
						
						foreach(string radioStation in Directory.GetDirectories( radioNetwork )) {

							RadioChannel radioChannel;

							if(!File.Exists(radioStation + "//RadioChannel.json")) {
								radioChannel = CreateRadioFromPath(radioStation, network.name);
							} else {
								radioChannel = JsonToRadio(radioStation, network.name);
							}
									
							//ExtendedRadio.AddAudioToDataBase(radioChannel);
							customeRadioChannelsName.Add(radioChannel.name);
							m_RadioChannels.Add(radioChannel.name, radioChannel.CreateRuntime(radioStation));					
						}
					}
				}
			}

            ExtendedRadio.radioTravers.Field("m_Networks").SetValue(m_Networks);
			ExtendedRadio.radioTravers.Field("m_RadioChannels").SetValue(m_RadioChannels);
			ExtendedRadio.radioTravers.Field("m_CachedRadioChannelDescriptors").SetValue(null);

            if (ExtendedRadioMod.s_setting.SaveLastRadio && ExtendedRadioMod.s_setting.LastRadio != null && m_RadioChannels.TryGetValue(ExtendedRadioMod.s_setting.LastRadio, out RuntimeRadioChannel channel))
            {
                ExtendedRadio.radio.currentChannel = channel;
            }

        }

		//internal static IEnumerator SearchForCustomRadiosFolder(List<string> ModsFolderPaths) {

		//	int progress = 0;
		//	int progressComplete = 1;

		//	var notificationInfo = MainSystem.m_NotificationUISystem.AddOrUpdateNotification(
		//		$"{nameof(ExtendedRadio)}.{nameof(CustomRadios)}.{nameof(SearchForCustomRadiosFolder)}", 
		//		title: "ExtendedRadio: searching custom radio.",
		//		progressState: ProgressState.Indeterminate, 
		//		progress: 0,
		//		thumbnail: $"{Icons.COUIBaseLocation}/Resources/DefaultIcon.svg"
		//	);

		//	foreach(string ModsFolderPath in ModsFolderPaths) {
		//		progressComplete += new DirectoryInfo(ModsFolderPath).GetDirectories().Count();
		//	}

		//	foreach(string ModsFolderPath in ModsFolderPaths) {

		//		notificationInfo.text = $"Searching custom radio in {Path.GetDirectoryName(ModsFolderPath)}.";
		//		notificationInfo.progressState = ProgressState.Progressing;

		//		foreach(DirectoryInfo directory in new DirectoryInfo(ModsFolderPath).GetDirectories()) {
		//			if(File.Exists($"{directory.FullName}\\CustomRadios.zip")) {
		//				if(Directory.Exists($"{directory.FullName}\\CustomRadios")) Directory.Delete($"{directory.FullName}\\CustomRadios", true);
		//				ZipFile.ExtractToDirectory($"{directory.FullName}\\CustomRadios", directory.FullName);
		//				File.Delete($"{directory.FullName}\\CustomRadios.zip");
		//			}
		//			if(Directory.Exists($"{directory.FullName}\\CustomRadios")) RegisterCustomRadioDirectory($"{directory.FullName}\\CustomRadios");
		//			progress++;
		//			notificationInfo.progress = (int)(progress / (float)progressComplete*100);
		//			yield return null;
		//		}
		//	}

		//	MainSystem.m_NotificationUISystem.RemoveNotification(
		//		identifier: notificationInfo.id, 
		//		delay: 3f, 
		//		text: $"Done, {radioDirectories.Count()} radio folder found.",
		//		progressState: ProgressState.Complete, 
		//		progress: 100
		//	);

		//}

		/// <summary>This methode add you folder that contains your radio to the list of radio to load.</summary>
		/// <param name="path">The global path to the folder that contains your custom radio</param>
		public static void RegisterCustomRadioDirectory(string path) {
			if (radioDirectories.Contains(path)) return;
			radioDirectories.Add(path);
			Icons.AddNewIconsFolder(new DirectoryInfo(path).Parent.FullName);			
		}

		public static void UnRegisterCustomRadioDirectory(string path) {
			radioDirectories.Remove(path);
			Icons.RemoveNewIconsFolder(new DirectoryInfo(path).Parent.FullName);
			ExtendedRadio.radio?.Reload();
		}

		/// <summary>This methode add your Network to the game.</summary>
		/// <param name="radioNetwork"> The RadioNetwork object you want to add to the game </param>
		/// <returns>True if the radio network was successfully added to the game, false otherwise.</returns>
		public static bool AddRadioNetworkToTheGame(RadioNetwork radioNetwork) {

			if(m_Networks.ContainsKey(radioNetwork.name)) return false;

			radioNetwork.uiPriority = radioNetworkIndex++;
			customeNetworksName.Add(radioNetwork.name);
			m_Networks.Add(radioNetwork.name, radioNetwork);

			return true;
		}
		/// <summary>Add your radio channel to the game.</summary>
		/// <param name="radioChannel">The radio channel to add.</param>
		/// <param name="path">(OPTIONAL) The global path to the radio channel, default = "".</param>
		/// <returns>True if the radio channel was successfully added to the game, false otherwise.</returns>
		public static bool AddRadioChannelToTheGame( RadioChannel radioChannel, string path = "") {

			if (customeRadioChannelsName.Contains(radioChannel.name)) return false;

			// ExtendedRadio.AddAudioToDataBase(radioChannel);
			customeRadioChannelsName.Add(radioChannel.name);
			m_RadioChannels.Add(radioChannel.name, radioChannel.CreateRuntime(path));

			return true;
		}

		/// <summary>Create a whole radio.</summary>
		/// <param name="path">Path to the folder that contain the RadioChannel.json</param>
		/// <param name="radioNetwork">(OPTIONAL) The name of the folder that contain the RadioNetwork.json</param>
		/// <returns>The RadioChannel with all the program/Segment and audio file.</returns>
		public static RadioChannel JsonToRadio(string path, string radioNetwork = null) {
			
			RadioChannel radioChannel = Decoder.Decode(File.ReadAllText(path+"\\RadioChannel.json")).Make<RadioChannel>();
			
			while (m_RadioChannels.ContainsKey(radioChannel.name))
			{
				radioChannel.name = radioChannel.name + "_" + ExtendedRadio.radioTravers.Method("MakeUniqueRandomName", radioChannel.name, 4).GetValue<string>();
			}

			radioChannel.nameId = radioChannel.name;

			if(radioNetwork != null) {
				radioChannel.network = radioNetwork;
			}

			if(Directory.GetFiles(Directory.GetDirectories( path )[0], "*.ogg").Length == 0 ) {
				foreach(string programDirectory in Directory.GetDirectories( path )) {

					Program program;

					if(File.Exists(programDirectory+"\\Program.json")) {
						program = Decoder.Decode(File.ReadAllText(programDirectory+"\\Program.json")).Make<Program>();
					} else {
						program = new() {
							name = new DirectoryInfo(radioNetwork).Name,
							description = new DirectoryInfo(radioNetwork).Name,
							icon = Icons.DefaultRadioIcon,
							startTime = "00:00",
							endTime = "00:00",
							loopProgram = true,
							pairIntroOutro = false
						};
					}

					foreach(string segmentDirectory in Directory.GetDirectories( programDirectory )) {

						Segment segment;

						if(File.Exists(segmentDirectory+"\\Segment.json")) {
							segment = Decoder.Decode(File.ReadAllText(segmentDirectory+"\\Segment.json")).Make<Segment>();
						} else {
							segment = new() {
								type = StringToSegmentType(new DirectoryInfo(radioNetwork).Name),
								tags = new string[0],
								clipsCap = 0,
							};
						}

						//segment.tags.AddRangeToArray([segment.type.ToString(), program.name, radioChannel.name, radioChannel.network]);
						segment.tags = FormatTags(segment.type, program.name, radioChannel.name, radioChannel.network);

                        //if(segment.tags.Length <= 0) {
                        //	segment.tags = [segment.type.ToString(), program.name, radioChannel.name, radioChannel.network];
                        //}

                        foreach (string audioAssetDirectory in Directory.GetDirectories(segmentDirectory))
                        {
                            segment.clips = segment.clips.AddRangeToArray(MusicLoader.LoadAudioFiles(audioAssetDirectory, segment.type, program.name, radioChannel.name, radioChannel.network));
                        }

						if(!File.Exists(segmentDirectory+"\\Segment.json")) segment.clipsCap = segment.clips.Length;

						program.segments = program.segments.AddToArray(segment);
					}

					radioChannel.programs = radioChannel.programs.AddToArray(program);
				}
			} else {
				radioChannel = CreateRadioFromPath(path, radioChannel.network, radioChannel);
			}

			return radioChannel;

		}


		static private RadioChannel CreateRadioFromPath(string path, string radioNetwork, RadioChannel radioChannel = null) {

			if(radioChannel == null) {

				string radioName = new DirectoryInfo(path).Name;

				string iconPath = Icons.DefaultRadioIcon;

				if(File.Exists(path+"\\icon.svg")) {
					iconPath = $"{Icons.COUIBaseLocation}/CustomRadios/{ new DirectoryInfo(path).Parent.Name}/{radioName}/icon.svg";
				}

				while (m_RadioChannels.ContainsKey(radioName))
				{
					radioName = radioName + "_" + ExtendedRadio.radioTravers.Method("MakeUniqueRandomName", radioName, 4).GetValue<string>();
				}

				radioChannel = new() {
					network = radioNetwork,
					name = radioName,
					nameId = radioName,
					description = radioName,
					icon = iconPath,
				};
			}

			Program program = new()
			{
				name = "My Custom Program",
				description = "My Custom Program",
				icon = Icons.DefaultRadioIcon,
				startTime = "00:00",
				endTime = "00:00",
				loopProgram = true
			};

			Segment segment = new()
			{
				type = SegmentType.Playlist,
				clipsCap = 0,
				clips = new Colossal.IO.AssetDatabase.AudioAsset[0],
				tags = FormatTags(SegmentType.Playlist, program.name, radioChannel.name, radioChannel.network)
			};

			segment.clips = segment.clips.AddRangeToArray(MusicLoader.LoadAudioFiles(path, segment.type, program.name, radioChannel.name, radioChannel.network));

            foreach (string audioAssetDirectory in Directory.GetDirectories( path )) {
                segment.clips = segment.clips.AddRangeToArray(MusicLoader.LoadAudioFiles(audioAssetDirectory, segment.type, program.name, radioChannel.name, radioChannel.network));
            }

			program.segments = new Segment[] { segment };
			radioChannel.programs = radioChannel.programs.AddToArray(program);



			return radioChannel;

		}

		/// <summary>Convert your JSON file to a RadioNetwork.</summary>
		/// <param name="path">Global path to the JSON file that contain the RadioNetwork</param>
		/// <returns>The RadioNetwork created from this JSON.</returns>
		public static RadioNetwork JsonToRadioNetwork(string path) {
			return Decoder.Decode(File.ReadAllText(path+"\\RadioNetwork.json")).Make<RadioNetwork>();
		}

		/// <summary>Convert your RadioNetwork to a JSON string.</summary>
		/// <param name="radioNetwork">The RadioNetwork you want to convert into a JSON string.</param>
		/// <returns>The RadioNetwork created from this JSON.</returns>
		public static string RadioNetworkToJson( RadioNetwork radioNetwork) {
			return Encoder.Encode(radioNetwork, EncodeOptions.None);
		}

		/// <summary>Convert your JSON file to a RadioChannel.</summary>
		/// <param name="path">Global path to the JSON file that contain the RadioChannel</param>
		/// <returns>The RadioChannel created from this JSON.</returns>
		public static RadioChannel JsonToRadioChannel(string path) {
			return Decoder.Decode(File.ReadAllText(path+"\\RadioChannel.json")).Make<RadioChannel>();
		}

		/// <summary>Convert your JSON file to a Program.</summary>
		/// <param name="path">Global path to the JSON file that contain the Program</param>
		/// <returns>The Program created from this JSON.</returns>
		public static Program JsonToProgram(string path) {
			return Decoder.Decode(File.ReadAllText(path+"\\Program.json")).Make<Program>();
		}

		/// <summary>Convert your JSON file to a Segment.</summary>
		/// <param name="path">Global path to the JSON file that contain the Segment</param>
		/// <returns>The Segment created from this JSON.</returns>
		public static Segment JsonToSegment(string path) {

			Segment segment = Decoder.Decode(File.ReadAllText(path+"\\Segment.json")).Make<Segment>();

			return segment;

		}

		public static string FormatTagSegmentType(SegmentType segmentType)
		{
			return $"type:{SegmentTypeToTypeTag(segmentType)}";
        }

        public static string FormatTagRadioProgram(string radioProgram)
        {
			return $"radio program:{radioProgram}";
        }

        public static string FormatTagRadioChannel(string radioChannel)
		{
			return $"radio channel:{radioChannel}";

        }

        public static string FormatTagRadioNetwork(string radioNetwork)
        {
			return $"radio network:{radioNetwork}";
        }

        public static string[] FormatTags(SegmentType segmentType, string channelName)
        {
			return FormatTags(segmentType, null, channelName, null);
        }

        public static string[] FormatTags(SegmentType segmentType, string radioProgram, string radioChannel, string radioNetwork)
		{
			return new string[] { FormatTagSegmentType(segmentType), FormatTagRadioProgram(radioProgram), FormatTagRadioChannel(radioChannel), FormatTagRadioNetwork(radioNetwork) };
		}

		/// <summary>Convert a string to a SegmentType.</summary>
		/// <param name="s">The string.</param>
		/// <returns>The SegmentType.</returns>
		public static SegmentType StringToSegmentType(string s) {
			return s switch
			{
				"Playlist" => SegmentType.Playlist,
				"Talkshow" => SegmentType.Talkshow,
				"PSA" => SegmentType.PSA,
				"Weather" => SegmentType.Weather,
				"News" => SegmentType.News,
				"Commercial" => SegmentType.Commercial,
				"Emergency" => SegmentType.Emergency,
				_ => SegmentType.Playlist,
			};
		}

		public static string SegmentTypeToTypeTag(SegmentType segmentType)
		{
            return segmentType switch
            {
                SegmentType.Playlist => "Music",
                SegmentType.Talkshow => SegmentType.Talkshow.ToString(), // "",
                SegmentType.PSA => "Public Service Announcements",
                SegmentType.Weather => SegmentType.Weather.ToString(),
                SegmentType.News => SegmentType.News.ToString(), //"News",
                SegmentType.Commercial => SegmentType.Commercial.ToString(), //"Commercial",
                SegmentType.Emergency => SegmentType.Emergency.ToString(),
				_ => "ERROR SegmentTypeToTypeTag(SegmentType segmentType)"
            };
        }
	}
}
