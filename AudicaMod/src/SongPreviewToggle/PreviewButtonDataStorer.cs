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
                //MelonLogger.Msg("deselected: " + transform.parent.name);
                previousbutton = this;

                return;
            }
            else
            {
                //MelonLogger.Msg("button was previously not pressed");
            }

            //garbage collection is evil so here is this
            try
            {
                if (previousbutton)
                {
                    //MelonLogger.Msg("deselected: " + previousbutton.transform.parent.name);

                    previousbutton.pressed = false;
                }
                else
                {
                    //MelonLogger.Msg("no previous button");
                }
            }
            catch (Exception e)
            {
                //MelonLogger.Msg("Exception removing previous button.");
            }
            finally
            {
                pressed = true;
                //MelonLogger.Msg("selected: " + transform.parent.name);
                previousbutton = this;
            }
        }

    }
}
