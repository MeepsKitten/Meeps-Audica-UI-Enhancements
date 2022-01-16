using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace AudicaModding.MeepsUIEnhancements
{
    class SongTimeUI
    {
        private static GameObject worldUiPrefab;
        public static GameObject localWorldPrefab = null;
        private static GameObject OverlayUiPrefab;
        public static GameObject localOverlayPrefab = null;
        public static string SongLength = null;
        public static int SongLengthMins = 0;
        public static int SongLengthSex = 0;


        private static void LoadAssets()
        {
            //World UI
            Il2CppAssetBundle Asset = Util.LoadAssets.LoadAssetData("UI Ehancements.src.SongTimeUI.timeui");
            worldUiPrefab = Asset.LoadAsset("Assets/Canvas.prefab").Cast<GameObject>();
            worldUiPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(worldUiPrefab);
            worldUiPrefab.SetActive(false);

            if (!worldUiPrefab.GetComponent<SongTimeUIManager>())
            {
                worldUiPrefab.AddComponent<SongTimeUIManager>();
            }

            //OverlayUI
            Asset = Util.LoadAssets.LoadAssetData("UI Ehancements.src.SongTimeUI.timedisplayoverlay");
            OverlayUiPrefab = Asset.LoadAsset("Assets/TimeDisplayOverlay.prefab").Cast<GameObject>();
            OverlayUiPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(OverlayUiPrefab);
            OverlayUiPrefab.SetActive(false);

            if (!OverlayUiPrefab.GetComponent<SongTimeOverlayUIManager>())
            {
                OverlayUiPrefab.AddComponent<SongTimeOverlayUIManager>();
            }

        }

        public static void Show()
        {
           
            if (MeepsUIEnhancements.songBrowserInstalled && IsEndlessActive()) return;

            if (!localWorldPrefab && !KataConfig.I.practiceMode)
            {
                if (MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.SongProgressEnabled)))
                    InitUI();
            }
            else if(!KataConfig.I.practiceMode)
            {
                localWorldPrefab.SetActive(true);
            }

            if (!MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.ShowProgressOnOverlay))) return;

            if (localOverlayPrefab)
            {
                localOverlayPrefab.SetActive(true);
            }
        }

        //a wrapper to prevent dll loading if not installed
        private static bool IsEndlessActive()
        {
            return PlaylistEndlessManager.EndlessActive;
        }

        public static void Hide()
        {
            if (localWorldPrefab)
            {
                localWorldPrefab.SetActive(false);
            }
            if (localOverlayPrefab)
            {
                localOverlayPrefab.SetActive(false);
            }
        }

        private static void InitUI()
        {
            if (localWorldPrefab || !MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.SongProgressEnabled)))
                return;


            LoadAssets();

            localWorldPrefab = GameObject.Instantiate(worldUiPrefab);
            localOverlayPrefab = GameObject.Instantiate(OverlayUiPrefab);

            localWorldPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            localOverlayPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            GameObject.DontDestroyOnLoad(localWorldPrefab);
            GameObject.DontDestroyOnLoad(localOverlayPrefab);

            var x = MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.ShowProgressOnRight)) ? 0.83f : 0.27f;
            localWorldPrefab.transform.localPosition = new Vector3(x, 1.24f, 1.492f);
            localWorldPrefab.transform.localScale = new Vector3(0.6f, 0.6f, 1);

            localWorldPrefab.SetActive(true);
            localOverlayPrefab.SetActive(MelonPreferences.GetEntryValue<bool>(Config.Config.CATegory, nameof(Config.Config.ShowProgressOnOverlay)));

        }

        [HarmonyPatch(typeof(InGameUI), "Restart")]
        private static class PatchRestart
        {
            private static void Prefix(InGameUI __instance)
            {
                Show();
            }
        }
    }
}
