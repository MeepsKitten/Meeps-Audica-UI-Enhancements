using System;
using MelonLoader;
using UnityEngine;
using Harmony;
using AudicaModding.MeepsUIEnhancements.AlbumArt;


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
                    if (MeepsUIEnhancements.songDataLoaderInstalled)
                    {
                        AlbumArt.AlbumArt.DisplayArt();
                    }
                }
                else
                {
                    if (MeepsUIEnhancements.songDataLoaderInstalled)
                    {
                        AlbumArt.AlbumArt.HideArt();
                    }
                }
            }
        }

    }
}
