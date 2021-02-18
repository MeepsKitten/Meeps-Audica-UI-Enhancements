using System;
using UnityEngine;
using UnityEngine.Events;

namespace AudicaModding.MeepsUIEnhancements.Util
{
    class ButtonUtilities
    {
        public class ButtonSettings
        {
            public bool destroyOnShot = false;
            public bool disableHighlightHaptics = false;
            public bool disableOnShot = false;
            public bool doHighlightSound = true;
            public bool doMeshExplosion = false;
            public bool doParticles = false;
            public string shootsound = "event:/shell/button_shatter";
        }
        
        public static void ObjectToButton(GameObject gameObject, Action callback, ButtonSettings settings)
        {
            var gunbutton = gameObject.AddComponent<GunButton>();
            gameObject.layer = 5; //UI layer           

            gunbutton.destroyOnShot = settings.destroyOnShot;
            gunbutton.disableHighlightHaptics = settings.disableHighlightHaptics;
            gunbutton.disableOnShot = settings.disableOnShot;
            gunbutton.doHighlightSound = settings.doHighlightSound;
            gunbutton.doMeshExplosion = settings.doMeshExplosion;
            gunbutton.doParticles = settings.doParticles;
            gunbutton.shootSound = settings.shootsound;


            gunbutton.onHitEvent = new UnityEvent();
            gunbutton.onHitEvent.AddListener(callback);

        }

        public static void ObjectToButton(GunButton gunbutton, Action callback, ButtonSettings settings)
        {      
            gunbutton.destroyOnShot = settings.destroyOnShot;
            gunbutton.disableHighlightHaptics = settings.disableHighlightHaptics;
            gunbutton.disableOnShot = settings.disableOnShot;
            gunbutton.doHighlightSound = settings.doHighlightSound;
            gunbutton.doMeshExplosion = settings.doMeshExplosion;
            gunbutton.doParticles = settings.doParticles;
            gunbutton.shootSound = settings.shootsound;


            gunbutton.onHitEvent = new UnityEvent();
            gunbutton.onHitEvent.AddListener(callback);

        }

    }
}
