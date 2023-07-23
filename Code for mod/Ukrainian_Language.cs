using BepInEx;
using BerryLoaderNS;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using System.Reflection;
using UnityEngine;
using HarmonyLib;

namespace Ukrainian_Language
{
    [BepInPlugin("ukrainian-language", "Ukrainian_Language", "1.0.0")]
    [BepInDependency("BerryLoader")]
    class Plugin : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource L;
        public static AssetBundle FontBundle;
        public static string BundlePath;
        private void Awake()
        {
            L = Logger;
            BundlePath = Path.Combine(Directory.GetParent(this.Info.Location).ToString(), "mod.bundle");
            Plugin.L.LogInfo(Directory.GetParent(this.Info.Location).ToString());
            var harmony = new Harmony("Plugin");
            harmony.PatchAll(typeof(Patches));


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
    class Patches
    {
        [HarmonyPatch(typeof(FontManager), "Awake")]
        [HarmonyPostfix]
        public static void FMAwake()
        {
            Plugin.L.LogInfo("Loading fixed font...");
            if (Plugin.FontBundle == null)
                Plugin.FontBundle = AssetBundle.LoadFromFile(Plugin.BundlePath);

            if (Plugin.FontBundle == null)
            {
                Plugin.L.LogError("Failed to load Font AssetBundle!");
                return;
            }

            FontManager.instance.WorldFontAsset = Plugin.FontBundle.LoadAsset<TMP_FontAsset>("NotoSans-Bold SDF");
            FontManager.instance.RegularFontAsset = Plugin.FontBundle.LoadAsset<TMP_FontAsset>("NotoSans-Bold SDF");
        }
    }
}