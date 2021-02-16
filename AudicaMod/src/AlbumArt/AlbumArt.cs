using System.Linq;
using MelonLoader;
using UnityEngine;
using Il2CppSystem.IO;
using UnityEngine.UI;
using System.Collections;
using Harmony;
using System;

namespace AudicaModding.MeepsUIEnhancements.AlbumArt
{
    public class AlbumArt
    {

        public static GameObject artObj;
        public static GameObject canvas;
        public static GameObject image;


        private static void EnableDisplayObject()
        {
            if(!artObj)
            {
                PrepareImagePrefab();
                MelonLogger.Log("Creating UI");
            }
            else
            {
                image.transform.localScale = new Vector3(0, 0, 0);
                artObj.SetActive(true);
            }

            MelonCoroutines.Start(EnterAnim());
        }

        private static IEnumerator EnterAnim()
        {
            yield return new WaitForSeconds(0.6f);

            float timer = 0;
            while (1 > image.transform.localScale.x)
            {
                timer += Time.deltaTime;
                image.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 2f;
                yield return null;
            }

        }

        private static IEnumerator ExitAnim()
        {
            float timer = 0;
            while (0 < image.transform.localScale.x)
            {
                timer += Time.deltaTime;
                image.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 3f;
                yield return null;
            }

            artObj.SetActive(false);
        }

        public static void DisplayArt()
        {
            Sprite currentAlbumArt = LoadCurrentSongSprite();
            if(currentAlbumArt == null)
            {
                MelonLogger.LogError("Album art sprite is null");
            }
            else
            {
                EnableDisplayObject();

                image.GetComponent<Image>().sprite = currentAlbumArt;

            }

        }

        public static void HideArt()
        {
            if(artObj)
            {
                MelonCoroutines.Start(ExitAnim());
            }
        }

        private static Sprite LoadCurrentSongSprite()
        {
            SongList.SongData song = SongDataHolder.I.songData;

            bool hasCustom = SongDataLoader.AllSongData[song.songID].HasCustomData();
            byte[] albumArtData = SongDataLoader.AllSongData[song.songID].albumArtData ;

            if (hasCustom && (albumArtData != null))
            {
                if (albumArtData.Length <= 0)
                {
                    MelonLogger.LogWarning("cover art data empty");
                    return null;
                }

                Sprite image = Util.LoadSprite.LoadSpriteFromDataArray(albumArtData);
                MelonLogger.Log("loaded sprite");
                return image;                           
            }
            else
            {
                MelonLogger.LogWarning("cover art data is null");
            }

            return null;
        }
        public static void PrepareImagePrefab()
        {
                //If I can figure out how to embedd the prefab ill use this
                /*var prefab = new MemoryStream(Properties.Resources.albumart);
                var prefabBundle = Il2CppAssetBundleManager.LoadFromStream(prefab);
                if (prefabBundle == null) MelonLogger.Log("Failed to load album art display asset, make sure you have installed the mod properly.");
                artObj = GameObject.Instantiate<GameObject>(prefabBundle.LoadAsset("Assets/AlbumArt.prefab").Cast<GameObject>());
                artObj.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                GameObject.DontDestroyOnLoad(artObj);
                artObj.SetActive(true);
                MelonLogger.Log("PrefabLoaded");*/

                //create objects
                artObj = new GameObject("Cover Art Obj");
                canvas = new GameObject("Cover Art Canvas");
                image = new GameObject("Cover Art Image");

                //make sure they stay
                artObj.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                canvas.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                image.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                GameObject.DontDestroyOnLoad(artObj);

                //set up hierarchy
                canvas.transform.SetParent(artObj.transform);
                image.transform.SetParent(canvas.transform);

                //add components
                canvas.AddComponent<Canvas>();
                image.AddComponent<CanvasRenderer>();
                image.AddComponent<Image>();

                //position canvas
                RectTransform canvasTransform = canvas.GetComponent<RectTransform>();
                canvasTransform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                canvasTransform.position += new Vector3(-5.2f, 2.3f, 24);

                 //set scales
                 image.transform.localScale = new Vector3(0, 0, 0);
            }

        private struct PreviousButtonPos
        {
            public Vector3 playbutt;
            public Vector3 playbuttlabel;
            public Vector3 chooseDifficultyLabel;
            public Vector3 difficultybutts;
        }

        private static PreviousButtonPos defaultButtonPos;
        private static bool defaultsSet = false;

        [HarmonyPatch(typeof(LaunchPanel), "OnEnable", new Type[0])]
        private static class LoadAlbumArt
        {
            private static void Postfix(LaunchPanel __instance)
            {
                SongList.SongData song = SongDataHolder.I.songData;

                bool hasCustom = SongDataLoader.AllSongData[song.songID].HasCustomData();
                if (hasCustom)
                {
                    byte[] art = SongDataLoader.AllSongData[song.songID].albumArtData;
                    if (art != null && art.Length > 0)
                    {
                        ValidateDefaults(__instance);

                        //set new
                        __instance.launchButton.transform.parent.localPosition = new Vector3(4.8f, 1.5f, 0);
                        //__instance.launchButtonLabel.transform.localPosition = new Vector3(1.6f, 0, -0.01f);
                        __instance.chooseDifficultyLabel.transform.localPosition = new Vector3(4.5f, -1.3f, 0);
                        __instance.expert.transform.localPosition = new Vector3(4.5f, -3.7f, 0);
                        __instance.hard.transform.localPosition = new Vector3(4.5f, -3.7f, 0);
                        __instance.normal.transform.localPosition = new Vector3(4.5f, -3.7f, 0);
                        __instance.easy.transform.localPosition = new Vector3(4.5f, -3.7f, 0);

                        return;
                    }
                }

                if (!defaultsSet)
                    return;

                //if no customs we want them to be in default position
                __instance.launchButton.transform.parent.position = defaultButtonPos.playbutt;
                //__instance.launchButtonLabel.transform.localPosition = defaultButtonPos.playbuttlabel;
                __instance.chooseDifficultyLabel.transform.localPosition = defaultButtonPos.chooseDifficultyLabel;
                __instance.expert.transform.localPosition = defaultButtonPos.difficultybutts;
                __instance.hard.transform.localPosition = defaultButtonPos.difficultybutts;
                __instance.normal.transform.localPosition = defaultButtonPos.difficultybutts;
                __instance.easy.transform.localPosition = defaultButtonPos.difficultybutts;
            }

            private static void ValidateDefaults(LaunchPanel __instance)
            {
                if (defaultsSet)
                    return;
                //save default pos
                defaultButtonPos.playbutt = __instance.launchButton.transform.parent.position;
                //defaultButtonPos.playbuttlabel = __instance.launchButtonLabel.transform.localPosition;
                defaultButtonPos.chooseDifficultyLabel = __instance.chooseDifficultyLabel.transform.localPosition;
                defaultButtonPos.difficultybutts = __instance.easy.transform.localPosition;

                defaultsSet = true;
            }
        }

    }
}
