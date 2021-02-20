using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AudicaModding.MeepsUIEnhancements.EasterEggs
{
    public class MeepsterEgg
    {
        public static GameObject Meeps = null;

        public static IEnumerator DownloadAsset(string downloadUrl, Action onDownloadComplete = null)
        {
            string[] splitURL = downloadUrl.Split('/');
            string audicaName = splitURL[splitURL.Length - 1];

            WWW www = new WWW(downloadUrl);
            yield return www;
            byte[] results = www.bytes;


            yield return null;

            var bundle = Il2CppAssetBundleManager.LoadFromMemory(results);
            GameObject egg = bundle.LoadAsset("Assets/Meepster Egg.prefab").Cast<GameObject>();
            Meeps = GameObject.Instantiate<GameObject>(egg);
            Meeps.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(Meeps);

            onDownloadComplete?.Invoke();
        }

        public static void ShowMeepsterEgg()
        {
            if(!Meeps)
                MelonCoroutines.Start(DownloadAsset("https://github.com/MeepsKitten/Meeps-Audica-UI-Enhancements/raw/main/AudicaMod/src/HostedAssets/meepsteregg", new Action(() => { ActivateAfterDL(); })));

            if (Meeps)
            {
                ActivateAfterDL();
            }
        }

        private static void ActivateAfterDL()
        {
            Meeps.SetActive(true);
            //Meeps.transform.Rotate(new Vector3(0, 0, 0), Space.World);
            Meeps.transform.localScale = new Vector3(35,35,35);
            Meeps.transform.localPosition = new Vector3(0, -20, 100);
        }

        public static void HideMeepsterEgg()
        {
            if(Meeps)
                Meeps.SetActive(false);
        }
    }
}
