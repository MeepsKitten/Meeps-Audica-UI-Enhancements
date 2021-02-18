using System;
using UnityEngine;

namespace AudicaModding.MeepsUIEnhancements.AlbumArt
{
    public class AlbumArtShoot : MonoBehaviour
    {
        public AlbumArtShoot(IntPtr intPtr) : base(intPtr) { }

        public void ButtonShot()
        {
            GetComponent<Animator>().Play("albumartshoot");

            AlbumArt.previewButton.onHitEvent.Invoke();
        }

    }
}
