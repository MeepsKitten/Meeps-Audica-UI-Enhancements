using System;
using MelonLoader;
using UnityEngine;
using Harmony;
using AudicaModding.MeepsUIEnhancements.AlbumArt;
using System.IO;
using UnityEngine.Events;

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


            }


        }

    }
}
