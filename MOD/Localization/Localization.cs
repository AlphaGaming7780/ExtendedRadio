using Colossal.Json;
using Colossal.Localization;
using Game.SceneFlow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ExtendedRadio;

public class Localization
{
    public static void LoadLocalization() 
	{
		try
		{
            foreach (string localeID in GameManager.instance.localizationManager.GetSupportedLocales())
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Dictionary<string, string> localization;

                if (assembly.GetManifestResourceNames().Contains($"{assembly.GetName().Name}.embedded.Localization.{localeID}.json"))
                    localization = Decoder.Decode(new StreamReader(assembly.GetManifestResourceStream($"{assembly.GetName().Name}.embedded.Localization.{localeID}.json")).ReadToEnd()).Make<Dictionary<string, string>>();
                else
                {
                    localization = Decoder.Decode(new StreamReader(assembly.GetManifestResourceStream($"{assembly.GetName().Name}.embedded.Localization.en-US.json")).ReadToEnd()).Make<Dictionary<string, string>>();
                }

                GameManager.instance.localizationManager.AddSource(localeID, new MemorySource(localization));
            }
        } catch (Exception ex) { Mod.log.Error(ex); }
	}
}

//[Serializable]
//internal class GlobaleLocalizationJS
//{
//	public Dictionary<string, Dictionary<string, string>> Localization = [];

//}

//[Serializable]
//internal class LocalLocalizationJS
//{
//	public Dictionary<string, string> Localization = [];
//}