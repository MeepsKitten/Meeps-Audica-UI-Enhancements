using Harmony;
using MelonLoader;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AudicaModding.MeepsUIEnhancements.Config
{
    public class Config
    {
        public const string CATegory = "U.I. Enhancements";

        public static string CoverArtSection = "[Header]Cover Art";
        public static bool CoverArt;
        public static float CoverArtBirghtness; 
        public static bool DefaultArt;

        public static string AdditionalElements = "[Header]Additional Elements";
        public static bool QuickDifficultyDisplay;
        public static bool PracticeModeMinimizationButton;

        public static string Effects = "[Header]Effects";
        public static float BloomAmount; //game default is: 5.24
        public static bool MeepsterEgg; 

        public static void RegisterConfig()
        {
            MelonPrefs.RegisterString(CATegory,nameof(CoverArtSection), "",CoverArtSection);

            MelonPrefs.RegisterBool(CATegory, nameof(CoverArt), true, "Display Cover Art");
            MelonPrefs.RegisterBool(CATegory, nameof(DefaultArt), true, "Display default art if there is none provided");
            MelonPrefs.RegisterFloat(CATegory, nameof(CoverArtBirghtness), 90, "Changes the brightness (value) of the album art [0,100,5,100]");

            MelonPrefs.RegisterString(CATegory, nameof(AdditionalElements),"", AdditionalElements);
            MelonPrefs.RegisterBool(CATegory, nameof(QuickDifficultyDisplay), true, "Display extra buttons to allow for quick difficulty selection");
            MelonPrefs.RegisterBool(CATegory, nameof(PracticeModeMinimizationButton), true, "Adds a button to minimize the practice mode UI");

            MelonPrefs.RegisterString(CATegory, nameof(Effects), "" ,Effects);
            MelonPrefs.RegisterFloat(CATegory, nameof(BloomAmount), 5.24f, "Changes the intensity of the bloom effect (the glow around stuff) [0,5.24,0.24,5.24]");
            MelonPrefs.RegisterBool(CATegory, nameof(MeepsterEgg), false, "???");

            OnModSettingsApplied();
        }

        

        public static void OnModSettingsApplied()
        {
            foreach (var fieldInfo in typeof(Config).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (fieldInfo.FieldType == typeof(int))
                    fieldInfo.SetValue(null, MelonPrefs.GetInt(CATegory, fieldInfo.Name));

                if (fieldInfo.FieldType == typeof(bool))
                {                  
                    fieldInfo.SetValue(null, MelonPrefs.GetBool(CATegory, fieldInfo.Name));

                    if((fieldInfo.Name == nameof(QuickDifficultyDisplay)) && !MelonPrefs.GetBool(CATegory, fieldInfo.Name))
                    {
                        if (QuickDifficultySelect.localprefab)
                            QuickDifficultySelect.localprefab.SetActive(false);
                    } else if((fieldInfo.Name == nameof(QuickDifficultyDisplay)))
                    {
                        if (QuickDifficultySelect.localprefab)
                            QuickDifficultySelect.localprefab.SetActive(true);
                    }

                    if((fieldInfo.Name == nameof(MeepsterEgg)) && MelonPrefs.GetBool(CATegory, fieldInfo.Name))
                    {
                        EasterEggs.MeepsterEgg.ShowMeepsterEgg();
                    }
                    else if(fieldInfo.Name == nameof(MeepsterEgg))
                    {
                        EasterEggs.MeepsterEgg.HideMeepsterEgg();
                    }
                }

                if (fieldInfo.FieldType == typeof(float))
                {
                    fieldInfo.SetValue(null, MelonPrefs.GetFloat(CATegory, fieldInfo.Name));                  
                }

            }
        }

        [HarmonyPatch(typeof(PostprocController), "UpdateBloom", new Type[0] )]
        private static class SetAlbumArtVisibility
        {
            private static void Prefix(PostprocController __instance)
            {
                __instance.mOriginalBloomIntensity = MelonPrefs.GetFloat(CATegory, nameof(BloomAmount));
            }
        }
    }
}