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

            try
            {
                if (previousbutton)
                {
                    MelonLogger.Log("deselected: " + previousbutton.transform.parent.name);

                    previousbutton.pressed = false;
                }
                else
                {
                    MelonLogger.Log("no previous button");
                }
            }
            catch (Exception e)
            {
                MelonLogger.Log("Exception removing previous button.");
            }
            finally
            {
                pressed = true;
                MelonLogger.Log("selected: " + transform.parent.name);
                previousbutton = this;
            }
        }

    }
}
