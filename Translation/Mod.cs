using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Ukrainian_Language
{
    public class Ukrainian_Language : Mod
    {
        public static AssetBundle FontBundle;
        public override void Ready()
        {
            var harmony = new Harmony("Ukrainian_Language");
            harmony.PatchAll(typeof(Patches));

            Logger.Log("Ready!");
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