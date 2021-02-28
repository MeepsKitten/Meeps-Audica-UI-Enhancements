﻿using Harmony;
using System;
using UnityEngine.Events;
using UnityEngine;

namespace AudicaModding.MeepsUIEnhancements
{
    class SongPreviewToggle
    {
        //Patch IsHighligted (which is used to tell if the preview should keep running)
        [HarmonyPatch(typeof(GunButton), "IsHighlighted", new Type[0] { })]
        private static class SongPreviewToggleOverride
        {
            private static bool Prefix(GunButton __instance, ref bool __result)
            {
              

                if (__instance.highlight != null)
                {
                    __result = __instance.highlight.activeInHierarchy;
                    
                }
                else
                {
                    __result = false;
                    return false;
                }

                PreviewButtonDataStorer buttstore = __instance.gameObject.GetComponentInChildren<PreviewButtonDataStorer>();
                if (buttstore)
                {
                    if (buttstore.pressed)
                    {
                        __result = true;
                        MelonLoader.MelonLogger.Log("Highlight ovveridden to true for: " + __instance.transform.parent.name);
                    }
                }

                return false;
            }

        }

        //Patch Init to add custom data storer and override the gunbutton script on the preview icon
        [HarmonyPatch(typeof(SongSelectItem), "Init", new Type[2] {typeof(string), typeof(SongSelect) })]
        private static class AddCustomPreviewButtonBehaviors
        {
            private static void Prefix(SongSelectItem __instance)
            {
                PreviewButtonDataStorer comp = __instance.songPreview.GetComponent<PreviewButtonDataStorer>();
                PreviewButtonDataStorer.previousbutton = null;

                if (!comp)
                {
                    comp = __instance.songPreview.gameObject.AddComponent<PreviewButtonDataStorer>();
                    comp.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                    GameObject.DontDestroyOnLoad(comp);
                }
                else
                {
                    comp.pressed = false;
                }

                __instance.songPreview.onHitEvent = new UnityEvent();
                Action shoot = new Action(() => { comp.PreviewButtonShoot(); });
                __instance.songPreview.onHitEvent.AddListener(shoot);

            }

        }


    }
}