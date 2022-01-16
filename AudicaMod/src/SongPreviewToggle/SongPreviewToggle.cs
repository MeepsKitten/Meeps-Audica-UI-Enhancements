using HarmonyLib;
using System;
using UnityEngine.Events;
using UnityEngine;
using MelonLoader;

namespace AudicaModding.MeepsUIEnhancements
{
    class SongPreviewToggle
    {
        //Patch IsHighligted (which is used to tell if the preview should keep running)
        [HarmonyPatch(typeof(GunButton), "IsHighlighted", new Type[0] { })]
        private static class SongPreviewToggleOverride
        {
            private static void Postfix(ref bool __result, GunButton __instance)
            {
                if (MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.SongPreviewToggle)))
                {
                    PreviewButtonDataStorer buttstore = __instance.gameObject.GetComponentInChildren<PreviewButtonDataStorer>();
                    if (buttstore)
                    {
                        if (buttstore.pressed)
                        {
                            __result = true;
                            //MelonLoader.MelonLogger.Msg("Highlight ovveridden to true for: " + __instance.transform.parent.name);
                        }
                    }
                }

            }
        }

        //Patch Init to add custom data storer and override the gunbutton script on the preview icon
        [HarmonyPatch(typeof(SongSelectItem), "Init", new Type[2] {typeof(string), typeof(SongSelect) })]
        private static class AddCustomPreviewButtonBehaviors
        {
            private static void Prefix(SongSelectItem __instance)
            {
                if (MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.SongPreviewToggle)))
                {
                    PreviewButtonDataStorer comp = __instance.songPreview.GetComponent<PreviewButtonDataStorer>();

                    if (!comp)
                    {
                        comp = __instance.songPreview.gameObject.AddComponent<PreviewButtonDataStorer>();
                        comp.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                        GameObject.DontDestroyOnLoad(comp);
                        comp.pressed = false;
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
}
