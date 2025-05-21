using Colossal.IO.AssetDatabase;
using Colossal.UI.Binding;
using ExtendedRadio.Extension;
using ExtendedRadio.UI;
using Game.UI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using static Game.Audio.Radio.Radio;

namespace ExtendedRadio
{
    internal partial class MixNetwork : UISystemBase
    {

        internal const string MixNetworkName = "Mix Network";

        public class RadioTag : IJsonWritable, IJsonReadable
        {
            public string Name;
            public string Tag;
            public SegmentType Type = SegmentType.Playlist;
            public List<RadioTag> RadioTags = new();

            public RadioTag(string name, string tag, SegmentType segmentType)
            {
                Name = name;
                Tag = tag;
                Type = segmentType;
            }

            public RadioTag(string name, string tag)
            {
                Name = name;
                Tag = tag;
            }

            public RadioTag(RadioTag radioTag)
            {
                Name = radioTag.Name;
                Tag = radioTag.Tag;
                Type = radioTag.Type;
                RadioTags = radioTag.RadioTags;
            }

            public RadioTag() { }

            public override bool Equals(object obj)
            {
                return obj is RadioTag tag &&
                       Tag == tag.Tag;
            }

            public override int GetHashCode()
            {
                return Tag.GetHashCode() + Name.GetHashCode() + RadioTags.GetHashCode();
            }

            public void Write(IJsonWriter writer)
            {
                writer.TypeBegin(GetType().FullName);
                writer.PropertyName("Name");
                writer.Write(Name);
                writer.PropertyName("Tag");
                writer.Write(Tag);
                writer.PropertyName("Type");
                new EnumNameWriter<SegmentType>().Write(writer, Type);
                //writer.Write(Type);
                writer.PropertyName("RadioTags");
                writer.Write(RadioTags);
                //writer.ArrayBegin(RadioTags.Count);
                //for (int i = 0; i < RadioTags.Count; i++)
                //{
                //    writer.Write(RadioTags[i]);
                //}
                //writer.ArrayEnd();
                writer.TypeEnd();
            }

            public void Read(IJsonReader reader)
            {
                reader.ReadMapBegin();
                reader.ReadProperty("Name");
                reader.Read(out Name);
                reader.ReadProperty("Tag");
                reader.Read(out Tag);
                reader.ReadProperty("Type");
                new EnumNameReader<SegmentType>().Read(reader, out Type);
                //reader.Read(out string segmentType);
                //Type = (SegmentType)segmentType;
                RadioTags = new List<RadioTag>();
                reader.ReadProperty("RadioTags");
                ulong size = reader.ReadArrayBegin();
                for (ulong i = 0; i < size; i++)
                {
                    RadioTag radioTag = new();
                    radioTag.Read(reader);
                    RadioTags.Add(radioTag);
                }
                reader.ReadArrayEnd();
                reader.ReadMapEnd();
            }
        }

        private static ValueBinding<List<RadioTag>> VB_radioTags;
        private static ValueBinding<Dictionary<SegmentType, List<string>>> VB_enabledTags;

        private static Dictionary<string, RadioNetwork> s_networks = new();
        private static Dictionary<string, RuntimeRadioChannel> s_radioChannels = new();
        internal static Dictionary<SegmentType, List<string>> s_enabledTags = ExtendedRadioMod.s_setting.EnabledTags;
        private static readonly List<RadioTag> s_radiosTags = new();

        protected override void OnCreate()
        {
            if(s_enabledTags.Count <= 0) s_enabledTags.Add(SegmentType.Playlist, new() { CustomRadios.FormatTagSegmentType(SegmentType.Playlist)  });
            base.OnCreate();
            AddBinding(VB_radioTags = new ValueBinding<List<RadioTag>>("extended_radio_mix", "radiotags", s_radiosTags, new ListWriter<RadioTag>(new ValueWriter<RadioTag>())));
            AddBinding(VB_enabledTags = new ValueBinding<Dictionary<SegmentType, List<string>>>("extended_radio_mix", "enabledtags", s_enabledTags, new DictionaryWriter<SegmentType, List<string>>( new EnumNameWriter<SegmentType>(), new ListWriter<string>())));
            AddBinding(new TriggerBinding<string, SegmentType > ("extended_radio_mix", "addtag", new Action<string, SegmentType>(Addtag), reader2: new EnumNameReader<SegmentType>()));
            AddBinding(new TriggerBinding<string, SegmentType > ("extended_radio_mix", "removetag", new Action<string, SegmentType>(RemoveTag), reader2: new EnumNameReader<SegmentType>()));
            VB_enabledTags.Update(s_enabledTags);
        }

        private void Addtag(string tag, SegmentType segmentType)
        {

            if ( s_enabledTags.ContainsKey(segmentType) && s_enabledTags[segmentType].Contains(tag))
            {
                ExtendedRadioMod.log.Warn($"The Tag is already in the dictionary : {segmentType} : {tag}");
                return;
            }

            if (!s_enabledTags.ContainsKey(segmentType)) s_enabledTags.Add(segmentType, new() { tag });
            else s_enabledTags[segmentType].Add(tag);
            UpdateTagsAndRadio();
        }

        private void RemoveTag(string tag, SegmentType segmentType)
        {

            if ( !s_enabledTags.ContainsKey(segmentType) || !s_enabledTags[segmentType].Contains(tag))
            {
                ExtendedRadioMod.log.Warn($"The Tag isn't in the dictionary : {segmentType} : {tag}");
                return;
            }

            s_enabledTags[segmentType].Remove(tag);

            if (s_enabledTags[segmentType].Count <= 0) s_enabledTags.Remove(segmentType);

            UpdateTagsAndRadio();
        }

        private void UpdateTagsAndRadio()
        {
            VB_enabledTags.Update(s_enabledTags);
            VB_enabledTags.TriggerUpdate();

            if(ExtendedRadioMod.s_setting.MixNetworkClearQueue)
            {
                if (ExtendedRadio.radio.currentClip.m_Emergency == Entity.Null && ExtendedRadioMod.s_setting.MixNetworkFinishCurrentClip)
                {
                    ExtendedRadio.radioTravers.Method("FinishCurrentClip").GetValue();
                }
                ExtendedRadio.radioTravers.Method("SetupOrSkipSegment").GetValue();
                ExtendedRadio.radioTravers.Method("ClearQueue", new[] { typeof(bool) }).GetValue(false);
                ExtendedRadio.radioTravers.Field("m_ReplayIndex").SetValue(0);
            }

            ExtendedRadioMod.s_setting.EnabledTags = s_enabledTags;
            ExtendedRadioMod.s_setting.ApplyAndSave();
        }

        internal static void CreateMixNetwork()
        {
            s_networks = ExtendedRadio.radioTravers.Field("m_Networks").GetValue<Dictionary<string, RadioNetwork>>();
            s_radioChannels = ExtendedRadio.radioTravers.Field("m_RadioChannels").GetValue<Dictionary<string, RuntimeRadioChannel>>();

            foreach (RadioNetwork network in s_networks.Values) network.uiPriority++;

            RadioNetwork mixNetwork = new()
            {
                name = MixNetworkName,
                nameId = null,
                description = "Listen to what you want when you want.",
                descriptionId = null,
                icon = Icons.MixNetworkIcon,
                allowAds = true,
                uiPriority = 0,//s_networks.Count,
            };

            Program mixProgram = new()
            {
                name = "Mix Program",
                description = "Mix Program Description",
                startTime = "00:00",
                endTime = "00:00",
                loopProgram = true,
                icon = Icons.MixNetworkIcon,
                pairIntroOutro = false,
                segments = new Segment[0]
            };

            foreach (SegmentType segmentType in Enum.GetValues(typeof(SegmentType)))
            {
                Segment segment = new()
                {
                    clips = new AudioAsset[0],
                    clipsCap = 1,
                    tags = new string[0],
                    type = segmentType,
                };
                mixProgram.segments = mixProgram.segments.AddItem(segment).ToArray();
            }

            RadioChannel mixChannel = new()
            {
                name = "Mix Channel",
                nameId = null,
                description = "Mix Channel description",
                icon = Icons.MixNetworkIcon,
                uiPriority = 0,
                network = mixNetwork.name,
                programs = new[] { mixProgram }
            };

            s_networks.Add(mixNetwork.name, mixNetwork);
            s_radioChannels.Add(mixChannel.name, mixChannel.CreateRuntime(""));

            ExtendedRadio.radioTravers.Field("m_Networks").SetValue(s_networks);
            ExtendedRadio.radioTravers.Field("m_RadioChannels").SetValue(s_radioChannels);
            ExtendedRadio.radioTravers.Field("m_CachedRadioChannelDescriptors").SetValue(null);

            s_networks = null;
            s_radioChannels = null;
        }

        internal static void UpdateRadioTags()
        {
            s_radioChannels = ExtendedRadio.radioTravers.Field("m_RadioChannels").GetValue<Dictionary<string, RuntimeRadioChannel>>();

            s_radiosTags.Clear();

            foreach (RuntimeRadioChannel channel in s_radioChannels.Values)
            {
                if (channel.network == MixNetworkName) continue;

                foreach (RuntimeProgram runtimeProgram in channel.schedule)
                {

                    foreach (RuntimeSegment runtimeSegment in runtimeProgram.segments)
                    {
                        RadioTag networkTag = new(channel.network, CustomRadios.FormatTagRadioNetwork(channel.network), runtimeSegment.type);
                        RadioTag channelTag = new(channel.name, CustomRadios.FormatTagRadioChannel(channel.name), runtimeSegment.type);
                        RadioTag programTag = new(runtimeProgram.name, CustomRadios.FormatTagRadioProgram(runtimeProgram.name), runtimeSegment.type);
                        RadioTag segmentTypeTag = new(CustomRadios.SegmentTypeToTypeTag(runtimeSegment.type), CustomRadios.FormatTagSegmentType(runtimeSegment.type), runtimeSegment.type);

                        if (channel.schedule.Length > 1) channelTag.RadioTags.Add(programTag);
                        networkTag.RadioTags.Add(channelTag);
                        segmentTypeTag.RadioTags.Add(networkTag);
                        

                        int index = s_radiosTags.IndexOf(segmentTypeTag);
                        if (index == -1)
                        {
                            s_radiosTags.Add(segmentTypeTag);
                            continue;
                        }

                        segmentTypeTag = s_radiosTags[index];
                        index = segmentTypeTag.RadioTags.IndexOf(networkTag);
                        if (index == -1)
                        {
                            segmentTypeTag.RadioTags.Add(networkTag);
                            continue;
                        }

                        networkTag = segmentTypeTag.RadioTags[index];
                        index = networkTag.RadioTags.IndexOf(channelTag);
                        if (index == -1)
                        {
                            networkTag.RadioTags.Add(channelTag);
                            continue;
                        }

                        if (channel.schedule.Length <= 1) continue;

                        channelTag = networkTag.RadioTags[index];
                        index = channelTag.RadioTags.IndexOf(programTag);
                        if (index == -1)
                        {
                            channelTag.RadioTags.Add(programTag);
                            continue;
                        }
                    }
                }
            }

            VB_radioTags.Update(s_radiosTags);
        }
    }
}
