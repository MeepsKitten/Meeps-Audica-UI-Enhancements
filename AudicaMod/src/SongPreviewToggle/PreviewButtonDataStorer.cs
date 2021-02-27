using System;
using UnityEngine;
using MelonLoader;

namespace AudicaModding.MeepsUIEnhancements
{
    public class PreviewButtonDataStorer: MonoBehaviour
    {
        public bool pressed = false;
        public static PreviewButtonDataStorer previousbutton;

        public PreviewButtonDataStorer(IntPtr intPtr) : base(intPtr) { }

        public void PreviewButtonShoot()
        {
            if(pressed)
            {
                pressed = false;
                MelonLogger.Log("deselected: " + transform.parent.name);
                previousbutton = this;

                return;
            }
            else
            {
                MelonLogger.Log("button was previously not pressed");
            }

            if (previousbutton)
            {
                previousbutton.pressed = false;
                MelonLogger.Log("deselected: " + previousbutton.transform.parent.name);
            }
            else
            {
                MelonLogger.Log("no previous button");
            }

            pressed = true;
            MelonLogger.Log("selected: " + transform.parent.name);
            previousbutton = this;
        }
    }
}
