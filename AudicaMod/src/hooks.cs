using System;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using AudicaModding.MeepsUIEnhancements.AlbumArt;
using System.IO;
using UnityEngine.Events;
using Hmx.Audio;
using System.Text.RegularExpressions;

namespace AudicaModding.MeepsUIEnhancements
{
    class hooks
    {       
        [HarmonyPatch(typeof(MenuState), "SetState", new Type[] { typeof(MenuState.State) })]
        private static class SetAlbumArtVisibility
        {
            private static void Postfix(MenuState __instance, ref MenuState.State state)
            {
                if (state == MenuState.State.LaunchPage)
                {

                    AlbumArt.AlbumArt.DisplayArt();

                    QuickDifficultySelect.InitUI();
                }
                else
                {
                    AlbumArt.AlbumArt.HideArt();

                }

                if (EasterEggs.MeepsterEgg.Meeps)
                    EasterEggs.MeepsterEgg.Meeps.GetComponentInChildren<Collider>().enabled = true;

                if(state == MenuState.State.Launched)
                {
                        SongTimeUI.Show();
                }
                else
                {
                    SongTimeUI.Hide();
                }

            }          

        }


        [HarmonyPatch(typeof(LaunchPanel), "Play", new Type[0])]
        private static class OnPlay
        {
            private static void Prefix(LaunchPanel __instance)
            {
                SongTimeUI.SongLength = GameObject.Find("menu/ShellPage_Launch/page/ShellPanel_Left/Canvas/duration").GetComponent<TMPro.TextMeshProUGUI>().text.Remove(0, 10);
                Regex expression = new Regex(@"(\d*):(\d*)");
                var results = expression.Matches(SongTimeUI.SongLength);

                SongTimeUI.SongLengthMins = Int32.Parse(results[0].Groups[1].Value);
                SongTimeUI.SongLengthSex = Int32.Parse(results[0].Groups[2].Value);
            }
        }

        [HarmonyPatch(typeof(SongPlayHistory), "RecordHistory", new Type[] { typeof(string), typeof(int), typeof(KataConfig.Difficulty), typeof(float), typeof(bool), typeof(bool) })]
        private static class AddAudicaScore
        {
            private static void Postfix(SongPlayHistory __instance, string songID, int score, KataConfig.Difficulty difficulty, float percent, bool fullCombo, bool noFail)
            {
                MeepsLogger.Msg("hide ui");

                SongTimeUI.Hide();
            }
        }


        //intercept sounds
        [HarmonyPatch(typeof(KataUtil), "PlayFMODEvent", new Type[] { typeof(string), typeof(UAudioEmitterCom) })]
        private static class InterceptSounds
        {
            private static bool Prefix(KataUtil __instance, string eventName, UAudioEmitterCom emitter)
            {
                if (eventName == "event:/gameplay/dodge_success" && MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.DisableMineSound)))
                {
                    return false;
                }
                else return true;
            }

        }

        /*[HarmonyPatch(typeof(GunButton), "NativeFieldInfoPtr_disableHighlightHaptics", new Type[0])]
        private static class InterceptHaptics
        {
            private static bool Prefix(bool __result, string eventName, UAudioEmitterCom emitter)
            {
                if (MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.DisableButtonHighlightHaptics)))
                {
                    __result = true;
                    return false;
                }
                else return true;
            }

        }
        */
    }
}
