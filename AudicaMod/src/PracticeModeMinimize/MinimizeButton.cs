using UnityEngine;
using AudicaModding.MeepsUIEnhancements.Util;
using HarmonyLib;
using System;
using MelonLoader;

namespace AudicaModding.MeepsUIEnhancements
{
    class PracticeModeMinimizeButton
    {
        public static GameObject obj_instance;
        public static GameObject staticObj;
        public static GunButton button;
        public static GameObject PracticeModeUI;

        public static void InitButton()
        {
            if(!staticObj)
            {
                staticObj = LoadAssets.ObjectFromAssetBundle("UI Ehancements.src.PracticeModeMinimize.arrowbutton", "Assets/scroll_arrow.prefab");
                button = staticObj.GetComponent<GunButton>();              
                button.highlight = staticObj.transform.GetChild(0).gameObject;
                staticObj.SetActive(false);
            }

            obj_instance = GameObject.Instantiate(staticObj);
            obj_instance.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(obj_instance);
            obj_instance.SetActive(true);

            button = obj_instance.GetComponent<GunButton>();
            ButtonUtilities.ButtonSettings settings = new ButtonUtilities.ButtonSettings();
            settings.shootsound = "event:/gameplay/no_look_success";
            ButtonUtilities.ObjectToButton(button, new Action(() => { OnShoot(); }), settings);

        }

        public static void OnShoot()
        {
            //this happens if you are minimizing
            if (PracticeModeUI.activeInHierarchy)
            {
                PracticeModeUI.SetActive(false);
                obj_instance.transform.localRotation = Quaternion.Euler(new Vector3(-15, 180, 0));
                button.shootSound = "event:/gameplay/enter_streak";
            }
            else //this happens if you are maximizing
            {
                PracticeModeUI.SetActive(true);
                obj_instance.transform.localRotation = Quaternion.Euler(new Vector3(-15, 180, 180));
                button.shootSound = "event:/gameplay/no_look_success";

            }
        }

        [HarmonyPatch(typeof(PracticeMode), "Awake", new Type[0] { })]
        private static class MinimizeButton
        {
            private static void Postfix(PracticeMode __instance)
            {
                if (MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.PracticeModeMinimizationButton)))
                {
                    PracticeModeUI = __instance.gameObject.transform.GetChild(0).gameObject;
                    InitButton();
                    obj_instance.transform.SetParent(__instance.gameObject.transform);
                    obj_instance.transform.localPosition = new Vector3(0.8f, 6.335f, 1.16f);
                    obj_instance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }
                else
                {
                    MeepsLogger.Msg("minimize button not enabled");
                }
            }
        }


    }
}
