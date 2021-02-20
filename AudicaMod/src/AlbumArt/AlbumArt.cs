using System.Linq;
using MelonLoader;
using UnityEngine;
using Il2CppSystem.IO;
using UnityEngine.UI;
using System.Collections;
using Harmony;
using System;
using AudicaModding.MeepsUIEnhancements.Util;

namespace AudicaModding.MeepsUIEnhancements.AlbumArt
{
    public class AlbumArt
    {

        public static GameObject artObj;
        public static GameObject canvas;
        public static GameObject image;
        public static GunButton previewButton;

        private static bool DefaultsSet = false;
        private static bool UsingCustomArt = false;
        private static Vector3 default_LaunchButtonPos;
        private static Vector3 default_DifficultyLabelPos;
        private static Vector3 default_DifficultyButtonPos;
        private static Vector3 default_DifficultyButtonScale;

        private static void EnableDisplayObject()
        {
            if (!artObj)
            {
                PrepareImagePrefab();
            }
            else
            {
                image.transform.parent.localScale = new Vector3(0, 0, 0);
                artObj.SetActive(true);
            }

            MelonCoroutines.Start(EnterAnim());
        }

        private static IEnumerator EnterAnim()
        {
            yield return new WaitForSeconds(0.6f);

            float timer = 0;
            while (1 > image.transform.parent.localScale.x)
            {
                timer += Time.deltaTime;
                image.transform.parent.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 2.5f;
                yield return null;
            }

        }

        private static IEnumerator ExitAnim()
        {
            float timer = 0;
            while (0 < image.transform.parent.localScale.x)
            {
                timer += Time.deltaTime;
                image.transform.parent.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 3f;
                yield return null;
            }

            artObj.SetActive(false);
        }

        public static void DisplayArt()
        {
            if (!MelonPrefs.GetBool(Config.Config.CATegory, nameof(Config.Config.CoverArt)))
                return;

            if (MeepsUIEnhancements.songDataLoaderInstalled)
            {
                var currentAlbumArt = LoadCurrentSongSprite();
                if (currentAlbumArt == null)
                {
                    EnableDisplayObject();             
                    image.GetComponent<Image>().sprite = MeepsUIEnhancements.defaultAlbumArt;
                    UsingCustomArt = false;
                }
                else
                {
                    EnableDisplayObject();

                    image.GetComponent<Image>().sprite = currentAlbumArt;
                    UsingCustomArt = true;


                }
            }
            else
            {
                EnableDisplayObject();
                image.GetComponent<Image>().sprite = MeepsUIEnhancements.defaultAlbumArt;
                UsingCustomArt = false;

            }

            //control cover art brightness
            float h, s, v;
            Color.RGBToHSV(image.GetComponent<Image>().color, out h, out s, out v);
            v = MelonPrefs.GetFloat(Config.Config.CATegory, nameof(Config.Config.CoverArtBirghtness));
            image.GetComponent<Image>().color = Color.HSVToRGB(h, s, v / 100);
        }

        public static void HideArt()
        {
            if (!MelonPrefs.GetBool(Config.Config.CATegory, nameof(Config.Config.CoverArt)))
                return;

            if (artObj)
            {
                MelonCoroutines.Start(ExitAnim());
            }
        }

        private static Sprite LoadCurrentSongSprite()
        {
            SongList.SongData song = SongDataHolder.I.songData;

            bool hasCustom = SongDataLoader.AllSongData[song.songID].HasCustomData();
            byte[] albumArtData = SongDataLoader.AllSongData[song.songID].albumArtData;

            if (hasCustom && (albumArtData != null))
            {
                if (albumArtData.Length <= 0)
                {
                    MelonLogger.LogWarning("cover art data empty");
                    return null;
                }

                Sprite image = Util.LoadSprite.LoadSpriteFromDataArray(albumArtData);
                return image;
            }
            else
            {
                //MelonLogger.LogWarning("cover art data is null");
            }

            return null;
        }
        public static void PrepareImagePrefab()
        {
            //create objects
            artObj = new GameObject("Cover Art Obj");
            canvas = new GameObject("Cover Art Canvas");


            var imageholder = Util.LoadAssets.ObjectFromAssetBundle("UI Ehancements.src.AlbumArt.albumart", "Assets/CoverArt.prefab");
            var cpnt = imageholder.AddComponent<AlbumArtShoot>();
            ButtonUtilities.ObjectToButton(imageholder, new Action(() => { cpnt.ButtonShot(); }), new ButtonUtilities.ButtonSettings());

            image = imageholder.transform.GetChild(0).gameObject;

            //make sure they stay
            artObj.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            canvas.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(artObj);

            //set up hierarchy
            canvas.transform.SetParent(artObj.transform);
            imageholder.transform.SetParent(canvas.transform);

            //add components
            canvas.AddComponent<Canvas>();

            //position canvas
            RectTransform canvasTransform = canvas.GetComponent<RectTransform>();
            canvasTransform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            canvasTransform.position += new Vector3(-5.2f, 2.15f, 24);

            //set scales
            imageholder.transform.localScale = new Vector3(0, 0, 0);
        }


        [HarmonyPatch(typeof(LaunchPanel), "OnEnable", new Type[0])]
        private static class LoadAlbumArt
        {
            private static void Postfix(LaunchPanel __instance)
            {

                var coverArtOn = MelonPrefs.GetBool(Config.Config.CATegory, nameof(Config.Config.CoverArt));
                var diffDispOn = MelonPrefs.GetBool(Config.Config.CATegory, nameof(Config.Config.QuickDifficultyDisplay));

                if(!DefaultsSet)
                {
                    default_LaunchButtonPos = __instance.launchButton.transform.parent.localPosition;
                    default_DifficultyLabelPos = __instance.chooseDifficultyLabel.transform.localPosition;
                    default_DifficultyButtonPos = __instance.expert.transform.localPosition;
                    default_DifficultyButtonScale = __instance.expert.transform.localScale;
                    DefaultsSet = true;
                }

             
                if (diffDispOn)
                {
                    __instance.launchButton.transform.parent.localPosition = new Vector3(__instance.launchButton.transform.parent.localPosition.x, 1.5f, __instance.launchButton.transform.parent.localPosition.z);
                    __instance.chooseDifficultyLabel.transform.localPosition = new Vector3(__instance.chooseDifficultyLabel.transform.localPosition.x, -0.7f, __instance.chooseDifficultyLabel.transform.localPosition.z);

                    //tf buttons
                    Vector3 translation = new Vector3(__instance.expert.transform.localPosition.x, -2.2f, __instance.expert.transform.localPosition.z);
                    __instance.expert.transform.localPosition = translation;
                    __instance.hard.transform.localPosition = translation;
                    __instance.normal.transform.localPosition = translation;
                    __instance.easy.transform.localPosition = translation;

                    //scale buttons
                    Vector3 scalevec = new Vector3(2.1f, 2.1f, 2.1f);
                    __instance.expert.transform.localScale = scalevec;
                    __instance.hard.transform.localScale = scalevec;
                    __instance.normal.transform.localScale = scalevec;
                    __instance.easy.transform.localScale = scalevec;
                }
                else if(DefaultsSet)
                {
                    __instance.launchButton.transform.parent.localPosition = new Vector3(__instance.launchButton.transform.parent.localPosition.x,default_LaunchButtonPos.y, __instance.launchButton.transform.parent.localPosition.z);
                    __instance.chooseDifficultyLabel.transform.localPosition = new Vector3(__instance.chooseDifficultyLabel.transform.localPosition.x, default_DifficultyLabelPos.y, __instance.chooseDifficultyLabel.transform.localPosition.z);

                    //tf buttons
                    Vector3 translation = new Vector3(__instance.expert.transform.localPosition.x,default_DifficultyButtonPos.y, __instance.expert.transform.localPosition.z);
                    __instance.expert.transform.localPosition = translation;
                    __instance.hard.transform.localPosition = translation;
                    __instance.normal.transform.localPosition = translation;
                    __instance.easy.transform.localPosition = translation;

                    //scale buttons
                    Vector3 scalevec = default_DifficultyButtonScale;
                    __instance.expert.transform.localScale = scalevec;
                    __instance.hard.transform.localScale = scalevec;
                    __instance.normal.transform.localScale = scalevec;
                    __instance.easy.transform.localScale = scalevec;
                }

                if (coverArtOn)
                {
                    __instance.launchButton.transform.parent.localPosition = new Vector3(4.8f, __instance.launchButton.transform.parent.localPosition.y, __instance.launchButton.transform.parent.localPosition.z);
                    __instance.chooseDifficultyLabel.transform.localPosition = new Vector3(4.8f, __instance.chooseDifficultyLabel.transform.localPosition.y, __instance.chooseDifficultyLabel.transform.localPosition.z);

                    //tf buttons
                    Vector3 translation = new Vector3(4.8f, __instance.expert.transform.localPosition.y, __instance.expert.transform.localPosition.z);
                    __instance.expert.transform.localPosition = translation;
                    __instance.hard.transform.localPosition = translation;
                    __instance.normal.transform.localPosition = translation;
                    __instance.easy.transform.localPosition = translation;

                    //save preview state
                    previewButton = __instance.songPreviewButton.GetComponentInChildren<GunButton>();
                }
                else if(DefaultsSet)
                {
                    ReturnCoverArtShift(__instance);
                }

                if(!MelonPrefs.GetBool(Config.Config.CATegory, nameof(Config.Config.DefaultArt)) && !UsingCustomArt)
                {
                    ReturnCoverArtShift(__instance);
                    artObj.SetActive(false);
                }

            }
        }

        private static void ReturnCoverArtShift(LaunchPanel __instance)
        {
            __instance.launchButton.transform.parent.localPosition = new Vector3(default_LaunchButtonPos.x, __instance.launchButton.transform.parent.localPosition.y, __instance.launchButton.transform.parent.localPosition.z);
            __instance.chooseDifficultyLabel.transform.localPosition = new Vector3(default_DifficultyLabelPos.x, __instance.chooseDifficultyLabel.transform.localPosition.y, __instance.chooseDifficultyLabel.transform.localPosition.z);

            Vector3 translation = new Vector3(default_DifficultyButtonPos.x, __instance.expert.transform.localPosition.y, __instance.expert.transform.localPosition.z);
            __instance.expert.transform.localPosition = translation;
            __instance.hard.transform.localPosition = translation;
            __instance.normal.transform.localPosition = translation;
            __instance.easy.transform.localPosition = translation;
        }
    }


}

