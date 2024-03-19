using System;

namespace ExtendedRadio
{	
	[Serializable]
    public class RadioAddon
    {	
		/// <summary>The name of the folder that contains the RadioNetwork.json of the Network tou want to add this addon./// </summary>
		public string RadioNetwork = null;
		/// <summary>The name of the folder that contains the RadioNetwork.json.</summary>
		public string RadioChannel = null;
		/// <summary>The name of the program</summary>
		public string Program = null;
		/// <summary>The the SegmentType</summary>
		public string SegmentType = null;
	}
}
 