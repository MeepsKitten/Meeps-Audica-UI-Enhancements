using Harmony;
using System;
using UnityEngine;

namespace AudicaModding.MeepsUIEnhancements.QuickDifficultySelect
{
    public class QuickDifficultySelect
    {
        private static GameObject prefab;
        private static GameObject localprefab = null;

        public static Il2CppAssetBundle Asset = null;

        public static void LoadAsset()
        {
            if (Asset == null)
            {
                Asset = Util.LoadAssets.LoadAssetData("UI Ehancements.src.QuickDifficultySelect.quickdiffbuttons");
            }
            prefab = Asset.LoadAsset("Assets/QuickDiffButtonManager.prefab").Cast<GameObject>();
            prefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(prefab);
            prefab.SetActive(false);

            if (!prefab.GetComponent<QuickDifficultyPanelManager>())
            {
                var comp = prefab.AddComponent<QuickDifficultyPanelManager>();
                comp.InitButtons();
            }

        }

        public static void InitUI()
        {
            if (localprefab)
                return;

            LoadAsset();


            var diffText = GameObject.Find("menu/ShellPage_Launch/page/ShellPanel_Center/choosediff");
            MelonLoader.MelonLogger.Log("diffText");

            localprefab = GameObject.Instantiate(prefab, diffText.transform);
            localprefab.GetComponent<QuickDifficultyPanelManager>().InitButtons();

            localprefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            GameObject.DontDestroyOnLoad(localprefab);

            localprefab.transform.localPosition = new Vector3(0, -2.5f, 0);
            localprefab.transform.localScale = new Vector3(0.6f, 0.6f, 1);

            localprefab.SetActive(true);

        }

        
    }
}
