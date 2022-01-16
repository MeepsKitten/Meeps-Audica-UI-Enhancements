using HarmonyLib;
using MelonLoader;
using System;
using UnityEngine;
using TMPro;

namespace AudicaModding.MeepsUIEnhancements
{
    class WatchController
    {
        public static GameObject watch;
        public static TextMeshProUGUI TimeDisplay;
        public static TextMeshProUGUI DateDisplay;
        public static TextMeshProUGUI SplashDisplay;
        private static string TimeTemplate;
        private static string DateTemplate;
        private static string SplashTemplate;

        /*public static void Update()
        {
            if (!MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.EnableWatch)) || !watch) { return; }


            var dateTime = DateTime.Now;

            //replace time text
            if (TimeDisplay)
            {
                var hour = MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.Use24HourTime)) ? dateTime.Hour.ToString(): dateTime.Hour == 0 ? "12" : dateTime.Hour > 12 ? (dateTime.Hour - 12).ToString() : dateTime.Hour.ToString();
                var minute = dateTime.Minute.ToString().PadLeft(2,'0');
                var ampm = MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.Use24HourTime)) ? "" : dateTime.ToString("tt").ToLower();

                var tmp = TimeTemplate;
                tmp = tmp.Replace("{h}", hour);
                tmp = tmp.Replace("{mm}", minute);
                tmp = tmp.Replace("{ap}", ampm);
                TimeDisplay.text = tmp;
            }

            if (DateDisplay)
            {
                var WeekDay = dateTime.DayOfWeek.ToString();
                var ShortWeekDay = dateTime.DayOfWeek.ToString().Substring(0,3);
                var Month = dateTime.Month.ToString();
                var Day = dateTime.Day.ToString();
                var ShortYear = dateTime.Year.ToString().Remove(0,2);
                var Year = dateTime.Year.ToString();

                var tmp = DateTemplate;

                tmp = tmp.Replace("{wd}", WeekDay);
                tmp = tmp.Replace("{swd}", ShortWeekDay);
                tmp = tmp.Replace("{m}", Month);
                tmp = tmp.Replace("{d}", Day);
                tmp = tmp.Replace("{sy}", ShortYear);
                tmp = tmp.Replace("{y}", Year);
                DateDisplay.text = tmp;
            }

            //TODO: Splash Text
        }

        [HarmonyPatch(typeof(TrackingSmoother), "Start", new Type[0])]
        private static class AttachWatchToHand
        {
            private static void Postfix(TrackingSmoother __instance)
            {
                if (!MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.EnableWatch)) || watch) return;

                switch (__instance.gameObject.name)
                {
                    case "LeftHandTracker":
                        SpawnWatch(true);
                        break;

                    case "RightHandTracker":
                        SpawnWatch();
                        break;
                    default:
                        return;
                }
            }
        }

        static void SpawnWatch(bool leftHand = false)
        {
            if(!MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.SwapToLeftHand)) && leftHand)
            {
                return;
            }
            //MelonLogger.Msg($"Spawning Watch. Is it on left? ->{leftHand}");
            GameObject handRoot;
            if (!leftHand)
            {
                handRoot = GameObject.Find("RightHandTracker/tracker/HandRight/vr_glove_right/vr_glove_model_audicahand/Root");
            }
            else
            {
                handRoot = GameObject.Find("LeftHandTracker/tracker/HandLeft/vr_glove_left/vr_glove_model_audicahand/Root");
            }

            watch = Util.LoadAssets.ChildObjectFromAssetBundle("UI Ehancements.src.Watch.watch", "Assets/Watch.prefab", handRoot.transform);

         
            var TempGO = Util.Frosting.RecursiveFindChild(watch.transform,"{Date}").gameObject;
            if(TempGO)
            {
                MeepsLogger.Msg("before date display");

                DateDisplay = TempGO.GetComponent<TextMeshProUGUI>();
                DateTemplate = DateDisplay.text;
            }

            TempGO = Util.Frosting.RecursiveFindChild(watch.transform, "{Time}").gameObject;
            if (TempGO)
            {
                TimeDisplay = TempGO.GetComponent<TextMeshProUGUI>();
                TimeTemplate = TimeDisplay.text;

            }

            TempGO = Util.Frosting.RecursiveFindChild(watch.transform, "{Splash}").gameObject;
            if (TempGO)
            {
                SplashDisplay = TempGO.GetComponent<TextMeshProUGUI>();
                SplashTemplate = SplashDisplay.text;

            }

            //TODO: test hand switching 
            if (leftHand)
            {
                //watch.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
            }
        }*/
    }
}
