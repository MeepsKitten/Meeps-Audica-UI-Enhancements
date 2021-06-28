using MelonLoader;
using UnityEngine;

namespace AudicaModding.MeepsUIEnhancements
{
    class SongTimeUI
    {
        private static GameObject prefab;
        public static GameObject localprefab = null;
        public static string SongLength = null;
        public static int SongLengthMins = 0;
        public static int SongLengthSex = 0;

        public static Il2CppAssetBundle Asset = null;

        private static void LoadAsset()
        {
            if (Asset == null)
            {
                Asset = Util.LoadAssets.LoadAssetData("UI Ehancements.src.SongTimeUI.timeui");
            }
            prefab = Asset.LoadAsset("Assets/Canvas.prefab").Cast<GameObject>();
            prefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(prefab);
            prefab.SetActive(false);

            if (!prefab.GetComponent<SongTimeUIManager>())
            {
                var comp = prefab.AddComponent<SongTimeUIManager>();
            }

        }

        public static void Show()
        {
            if (!localprefab)
            {
                if(MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.SongProgressEnabled)))
                    InitUI();
            }
            else
            {
                localprefab.SetActive(true);
            }
        }

        public static void Hide()
        {
            if(localprefab)
            {
                localprefab.SetActive(false);
            }
        }

        private static void InitUI()
        {
            if (localprefab || !MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.SongProgressEnabled)))
                return;

            LoadAsset();

            localprefab = GameObject.Instantiate(prefab);

            localprefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            GameObject.DontDestroyOnLoad(localprefab);

            var x = MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.ShowProgressOnRight)) ? 0.83f : 0.27f;
            localprefab.transform.localPosition = new Vector3(x, 1.24f, 1.492f);
            localprefab.transform.localScale = new Vector3(0.6f, 0.6f, 1);

            localprefab.SetActive(true);

        }



    }
}
