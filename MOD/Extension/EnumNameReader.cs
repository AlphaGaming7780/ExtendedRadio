using Colossal.UI.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.Audio.Radio.Radio;

namespace ExtendedRadio.Extension;

public class EnumNameReader<T> : IReader<T> where T : Enum
{
    public void Read(IJsonReader reader, out T value)
    {
        reader.Read(out string value2);
        value = (T)Enum.Parse(typeof(T), value2);
    }
}