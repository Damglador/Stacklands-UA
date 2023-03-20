using BepInEx;
using BerryLoaderNS;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace VietnameseTranslationMod
{
    [BepInPlugin("ukrainian-translate", "Translate text into Ukrainian", "0.0.2")]
    [BepInDependency("BerryLoader")]
    class Plugin : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource L;

        private void Awake()
        {
            L = Logger;

            var translationFilePath = Path.Combine(Directory.GetParent(this.Info.Location).FullName, "translation.tsv");

            if (!File.Exists(translationFilePath))
            {
                L.LogError("translation.tsv missing!");
            }
            else
            {
                LocAPI.LoadTsvFromFile(translationFilePath);
            }

            var langs = SokLoc.Languages.ToList<SokLanguage>();
            langs.Add(new SokLanguage { LanguageName = "Ukrainian", UnitySystemLanguage = SystemLanguage.Ukrainian });
            SokLoc.Languages = langs.ToArray();

            var langNamesField = typeof(SokLoc).GetField("localLanguageNames", BindingFlags.NonPublic | BindingFlags.Static);
            Dictionary<string, string> langNames = (Dictionary<string, string>)langNamesField.GetValue(null);
            langNames.Add("Ukrainian", "Українська");
            langNamesField.SetValue(null, langNames);
        }
    }
}