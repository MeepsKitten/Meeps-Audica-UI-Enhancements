using System;
using System.Collections.Generic;
using UnityEngine;
using AudicaModding.MeepsUIEnhancements.Util;
using Harmony;

namespace AudicaModding.MeepsUIEnhancements.QuickDifficultySelect
{
    public class QuickDifficultyPanelManager : MonoBehaviour
    {
        public QuickDifficultyPanelManager(IntPtr intPtr) : base(intPtr) { }

        public static GameObject bg;
        public static GameObject EasyButton;
        public static GameObject NormalButton;
        public static GameObject HardButton;
        public static GameObject ExpertButton;

        public static GunButton EasyButton_s;
        public static GunButton NormalButton_s;
        public static GunButton HardButton_s;
        public static GunButton ExpertButton_s;

        public Dictionary<string, KataConfig.Difficulty> nameToKataDiff = new Dictionary<string, KataConfig.Difficulty>
        {
            {"easy", KataConfig.Difficulty.Easy},
            {"expert", KataConfig.Difficulty.Expert},
            {"hard", KataConfig.Difficulty.Hard},
            {"medium", KataConfig.Difficulty.Normal}
        };


        public void ButtonShot(GameObject button)
        {
            KataConfig.I.SetDifficulty(nameToKataDiff[button.name]);
        }    

        public void Awake()
        {
            InitButtons();

            //assign actions
            ButtonUtilities.ButtonSettings settings = new ButtonUtilities.ButtonSettings();
            settings.doMeshExplosion = true;
            ButtonUtilities.ObjectToButton(EasyButton_s, new Action(() => { ButtonShot(EasyButton); }), settings);
            ButtonUtilities.ObjectToButton(ExpertButton_s, new Action(() => { ButtonShot(ExpertButton); }), settings);
            ButtonUtilities.ObjectToButton(NormalButton_s, new Action(() => { ButtonShot(NormalButton); }), settings);
            ButtonUtilities.ObjectToButton(HardButton_s, new Action(() => { ButtonShot(HardButton); }), settings);


            //set mats
            var expertstarmedal = GameObject.Find("menu/ShellPage_DifficultySelect/page/ShellPanel_Center/DifficultySelectButton_expert/star_medal");
            ExpertButton.GetComponent<Renderer>().material = expertstarmedal.GetComponent<Renderer>().material;
            var hardstarmedal = GameObject.Find("menu/ShellPage_DifficultySelect/page/ShellPanel_Center/DifficultySelectButton_hard/star_medal");
            HardButton.GetComponent<Renderer>().material = hardstarmedal.GetComponent<Renderer>().material;
            var normalstarmedal = GameObject.Find("menu/ShellPage_DifficultySelect/page/ShellPanel_Center/DifficultySelectButton_normal/star_medal");
            NormalButton.GetComponent<Renderer>().material = normalstarmedal.GetComponent<Renderer>().material;
            var easystarmedal = GameObject.Find("menu/ShellPage_DifficultySelect/page/ShellPanel_Center/DifficultySelectButton_easy/star_medal");
            EasyButton.GetComponent<Renderer>().material = easystarmedal.GetComponent<Renderer>().material;

        }

        public void InitButtons()
        {
            //get buttons
            bg = gameObject.transform.GetChild(0).gameObject;
            EasyButton = bg.transform.GetChild(0).gameObject;
            NormalButton = bg.transform.GetChild(1).gameObject;
            HardButton = bg.transform.GetChild(2).gameObject;
            ExpertButton = bg.transform.GetChild(3).gameObject;

            //get Gun button scripts
            EasyButton_s = EasyButton.GetComponent<GunButton>();
            ExpertButton_s = ExpertButton.GetComponent<GunButton>();
            NormalButton_s = NormalButton.GetComponent<GunButton>();
            HardButton_s = HardButton.GetComponent<GunButton>();
        }

        [HarmonyPatch(typeof(LaunchPanel), "OnEnable", new Type[0])]
        private static class SetActiveButtons
        {
            private static void Postfix(LaunchPanel __instance)
            {
                var songData = SongDataHolder.I.songData;

                //set button visibility based on availible diffs
                EasyButton.GetComponent<MeshRenderer>().enabled = songData.hasEasy;
                ExpertButton.GetComponent<MeshRenderer>().enabled = songData.hasExpert;
                HardButton.GetComponent<MeshRenderer>().enabled = songData.hasHard;
                NormalButton.GetComponent<MeshRenderer>().enabled = songData.hasNormal;

                //set interactivity based onb avalible diffs
                EasyButton_s.SetInteractable(songData.hasEasy);
                ExpertButton_s.SetInteractable(songData.hasExpert);
                HardButton_s.SetInteractable(songData.hasHard);
                NormalButton_s.SetInteractable(songData.hasNormal);

            }
        }

    }
}
