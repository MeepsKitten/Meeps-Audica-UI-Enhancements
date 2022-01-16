using HarmonyLib;
using MelonLoader;
using System;
using System.Reflection;
using UnityEngine;

namespace AudicaModding.MeepsUIEnhancements.Config
{
    public class Config
    {
        public const string CATegory = "UX Enhancements";

        public static string CoverArtSection;
        public static bool CoverArt;
        public static float CoverArtBirghtness;
        public static bool DefaultArt;

        public static string AdditionalElements;
        public static bool QuickDifficultyDisplay;
        public static bool PracticeModeMinimizationButton;

        public static string SongProgressUISection;
        public static bool SongProgressEnabled;
        public static bool ShowProgressOnOverlay;
        public static bool ShowProgressOnRight;
        public static bool ShowProgressAsPercentage;

        public static string WatchSettings;
        public static bool EnableWatch;
        public static bool SwapToLeftHand;
        public static bool Use24HourTime;


        public static string Tweaks;
        public static bool HideOldDifficultyButton;
        public static bool DisableMineSound;
        public static bool DisableButtonHighlightHaptics;
        public static bool CenterDifficultyButtonText;
        public static bool SongPreviewToggle;
        public static float BloomAmount; //game default is: 5.24
        public static bool MeepsterEgg;

        public static void RegisterConfig()
        {
            MelonPreferences.CreateEntry(CATegory, nameof(CoverArtSection), "", "[Header]Cover Art");

            MelonPreferences.CreateEntry(CATegory, nameof(CoverArt), true, "Display Cover Art");
            MelonPreferences.CreateEntry(CATegory, nameof(DefaultArt), true, "Display default art if there is none provided");
            MelonPreferences.CreateEntry(CATegory, nameof(CoverArtBirghtness), 90.0f, "Changes the brightness (value) of the album art [0,100,5,100]");

            MelonPreferences.CreateEntry(CATegory, nameof(AdditionalElements), "", "[Header]Additional Elements");
            MelonPreferences.CreateEntry(CATegory, nameof(QuickDifficultyDisplay), true, "Display extra buttons to allow for quick difficulty selection");
            MelonPreferences.CreateEntry(CATegory, nameof(PracticeModeMinimizationButton), true, "Adds a button to minimize the practice mode UI");

            MelonPreferences.CreateEntry(CATegory, nameof(SongProgressUISection), "", "[Header]Song Progress Display");
            MelonPreferences.CreateEntry(CATegory, nameof(SongProgressEnabled), true, "Enables a UI element that shows song progress in numerical format above the progress bar");
            MelonPreferences.CreateEntry(CATegory, nameof(ShowProgressOnOverlay), true, "The progress display will show as an overlay on the desktop window for the game");
            MelonPreferences.CreateEntry(CATegory, nameof(ShowProgressOnRight), false, "Moves the progress UI to the right side of the score display instead of the left");
            MelonPreferences.CreateEntry(CATegory, nameof(ShowProgressAsPercentage), false, "Changes the Song Progress Display to show a percentage of the song completed (as opposed to exact time values)");

           /*MelonPreferences.CreateEntry(CATegory, nameof(WatchSettings), "", "[Header]Watch Settings");
            MelonPreferences.CreateEntry(CATegory, nameof(EnableWatch), true, "Puts a watch on your right hand");
            //MelonPreferences.CreateEntry(CATegory, nameof(SwapToLeftHand), false, "Swaps the watch to be on your left hand");
            MelonPreferences.CreateEntry(CATegory, nameof(Use24HourTime), false, "Time will now be represented using 24 hour time representation");*/


            MelonPreferences.CreateEntry(CATegory, nameof(Tweaks), "", "[Header]Tweaks");
            MelonPreferences.CreateEntry(CATegory, nameof(DisableMineSound), true, "Disables the sound of mines passing you");
            //MelonPreferences.CreateEntry(CATegory, nameof(DisableButtonHighlightHaptics), false, "Disables the haptics when highlighting a button");
            MelonPreferences.CreateEntry(CATegory, nameof(HideOldDifficultyButton), false, "If using you're the \"Quick Difficulty Display\" this will hide the old button");
            MelonPreferences.CreateEntry(CATegory, nameof(CenterDifficultyButtonText), true, "Centers the difficulty name text in it's text box");
            MelonPreferences.CreateEntry(CATegory, nameof(SongPreviewToggle), true, "Allows you to shoot the song preview icon to keep the audio playing without needing to continue to hover over it");
            MelonPreferences.CreateEntry(CATegory, nameof(BloomAmount), 5.24f, "Changes the intensity of the bloom effect (the glow around stuff) [0,10.48,0.24,5.24]");
            MelonPreferences.CreateEntry(CATegory, nameof(MeepsterEgg), false, "be prepared for a huge lag spike");

            OnPreferencesSaved();
        }



        public static void OnPreferencesSaved()
        {
            foreach (var fieldInfo in typeof(Config).GetFields(BindingFlags.Static | BindingFlags.Public))
            {               

                if (fieldInfo.FieldType == typeof(int))
                    fieldInfo.SetValue(null, MelonPreferences.GetEntryValue<int>(CATegory, fieldInfo.Name));

                if (fieldInfo.FieldType == typeof(bool))
                {
                    fieldInfo.SetValue(null, MelonPreferences.GetEntryValue<bool>(CATegory, fieldInfo.Name));

                    if ((fieldInfo.Name == nameof(QuickDifficultyDisplay)) && !MelonPreferences.GetEntryValue<bool>(CATegory, fieldInfo.Name))
                    {
                        if (QuickDifficultySelect.localprefab)
                            QuickDifficultySelect.localprefab.SetActive(false);
                    }
                    else if ((fieldInfo.Name == nameof(QuickDifficultyDisplay)))
                    {
                        if (QuickDifficultySelect.localprefab)
                            QuickDifficultySelect.localprefab.SetActive(true);
                    }

                    if ((fieldInfo.Name == nameof(MeepsterEgg)) && MelonPreferences.GetEntryValue<bool>(CATegory, fieldInfo.Name))
                    {
                        EasterEggs.MeepsterEgg.ShowMeepsterEgg();
                    }
                    else if (fieldInfo.Name == nameof(MeepsterEgg))
                    {
                        EasterEggs.MeepsterEgg.HideMeepsterEgg();
                    }

                    if (fieldInfo.Name == nameof(ShowProgressOnRight))
                    {
                        var x = MelonPreferences.GetEntryValue<bool>(CATegory, nameof(ShowProgressOnRight)) ? 0.83f : 0.27f;
                        if(SongTimeUI.localWorldPrefab)
                            SongTimeUI.localWorldPrefab.transform.localPosition = new Vector3(x, 1.24f, 1.492f);
                    }

                    if(fieldInfo.Name == nameof(SwapToLeftHand))
                    {
                        var leftHand = MelonPreferences.GetEntryValue<bool>(CATegory, nameof(SwapToLeftHand));
                        if (!WatchController.watch) return;
                        //WatchController.watch.transform.localRotation = Quaternion.EulerRotation(leftHand ? new Vector3(0, 180, 0) : Vector3.zero);
                    }

                }

                if (fieldInfo.FieldType == typeof(float))
                {
                    fieldInfo.SetValue(null, MelonPreferences.GetEntryValue<float>(CATegory, fieldInfo.Name));
                }

            }
        }

        [HarmonyPatch(typeof(PostprocController), "UpdateBloom", new Type[0])]
        private static class SetAlbumArtVisibility
        {
            private static void Prefix(PostprocController __instance)
            {
                __instance.mOriginalBloomIntensity = MelonPreferences.GetEntryValue<float>(CATegory, nameof(BloomAmount));
            }
        }

        [HarmonyPatch(typeof(LaunchPanel), "OnEnable", new Type[0])]
        private static class SetOldDiffButtonStuff
        {
            private static void Postfix(LaunchPanel __instance)
            {
                bool active = !(MelonPreferences.GetEntryValue<bool>(CATegory, nameof(HideOldDifficultyButton)) && MelonPreferences.GetEntryValue<bool>(CATegory, nameof(QuickDifficultyDisplay)));

                //set active state of old button renderer based on prefs
                __instance.expert.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().enabled = active;
                __instance.hard.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().enabled = active;
                __instance.normal.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().enabled = active;
                __instance.easy.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().enabled = active;

                //set active state of old button collider based on prefs
                __instance.expert.transform.GetChild(0).GetComponent<UnityEngine.Collider>().enabled = active;
                __instance.hard.transform.GetChild(0).GetComponent<UnityEngine.Collider>().enabled = active;
                __instance.normal.transform.GetChild(0).GetComponent<UnityEngine.Collider>().enabled = active;
                __instance.easy.transform.GetChild(0).GetComponent<UnityEngine.Collider>().enabled = active;

               
                //set text alignment on diff name based on prefs
                TMPro.TextAlignmentOptions align = TMPro.TextAlignmentOptions.MidlineLeft;
                if (MelonPreferences.GetEntryValue<bool>(CATegory, nameof(CenterDifficultyButtonText)))
                {
                    align = TMPro.TextAlignmentOptions.Center;
                }

                __instance.expert.transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().m_textAlignment = align;
                __instance.hard.transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().m_textAlignment = align;
                __instance.normal.transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().m_textAlignment = align;
                __instance.easy.transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().m_textAlignment = align;
            }
        }
    }
}