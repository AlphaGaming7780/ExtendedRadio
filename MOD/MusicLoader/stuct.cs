using System;

namespace ExtendedRadio
{
	/// <summary>The class that contain all the info for the audio json.</summary>
				
	[Serializable]
	public class JsonAudioAsset
	{
		//public string AudioFileFormat = "OGG";
		public string Title = "";
		public string Album = "";
		public string Artist = "";
		public string Type = "";
		public string Brand = "Pilotee";
		//public string RadioStation = null;
		//public string RadioChannel = null;
		public string PSAType = "";
		public string AlertType = "";
		public string NewsType = "";
		public string WeatherType = "";
		//public double loopStart = -1;
		//public double loopEnd = -1;
		//public double alternativeStart = -1;
		//public float fadeoutTime = 1f;
		public string[] tags = [];
	}
}
 