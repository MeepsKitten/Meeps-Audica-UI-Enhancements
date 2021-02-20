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

        public class DiffButt
        {
            public GameObject Button;
            public GunButton ButtonScript;
        }

        public static GameObject bg;
        public static DiffButt Easy = new DiffButt();
        public static DiffButt Normal = new DiffButt();
        public static DiffButt Hard = new DiffButt();
        public static DiffButt Expert = new DiffButt();

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
            ButtonUtilities.ObjectToButton(Easy.ButtonScript, new Action(() => { ButtonShot(Easy.Button); }), settings);
            ButtonUtilities.ObjectToButton(Expert.ButtonScript, new Action(() => { ButtonShot(Expert.Button); }), settings);
            ButtonUtilities.ObjectToButton(Normal.ButtonScript, new Action(() => { ButtonShot(Normal.Button); }), settings);
            ButtonUtilities.ObjectToButton(Hard.ButtonScript, new Action(() => { ButtonShot(Hard.Button); }), settings);


            //set mats
            var expertstarmedal = GameObject.Find("menu/ShellPage_DifficultySelect/page/ShellPanel_Center/DifficultySelectButton_expert/star_medal");
            Expert.Button.GetComponent<Renderer>().material = expertstarmedal.GetComponent<Renderer>().material;
            var hardstarmedal = GameObject.Find("menu/ShellPage_DifficultySelect/page/ShellPanel_Center/DifficultySelectButton_hard/star_medal");
            Hard.Button.GetComponent<Renderer>().material = hardstarmedal.GetComponent<Renderer>().material;
            var normalstarmedal = GameObject.Find("menu/ShellPage_DifficultySelect/page/ShellPanel_Center/DifficultySelectButton_normal/star_medal");
            Normal.Button.GetComponent<Renderer>().material = normalstarmedal.GetComponent<Renderer>().material;
            var easystarmedal = GameObject.Find("menu/ShellPage_DifficultySelect/page/ShellPanel_Center/DifficultySelectButton_easy/star_medal");
            Easy.Button.GetComponent<Renderer>().material = easystarmedal.GetComponent<Renderer>().material;

            //set highlight
            Easy.ButtonScript.highlight = Easy.Button.transform.GetChild(1).gameObject;
            Hard.ButtonScript.highlight = Hard.Button.transform.GetChild(1).gameObject;
            Normal.ButtonScript.highlight = Normal.Button.transform.GetChild(1).gameObject;
            Expert.ButtonScript.highlight = Expert.Button.transform.GetChild(1).gameObject;
        }

        public void InitButtons()
        {
            //get buttons


            bg = gameObject.transform.GetChild(0).gameObject;

            Easy.Button = bg.transform.GetChild(0).gameObject;
            Normal.Button = bg.transform.GetChild(1).gameObject;
            Hard.Button = bg.transform.GetChild(2).gameObject;
            Expert.Button = bg.transform.GetChild(3).gameObject;

            //get Gun button scripts
            Easy.ButtonScript = Easy.Button.GetComponent<GunButton>();
            Expert.ButtonScript = Expert.Button.GetComponent<GunButton>();
            Normal.ButtonScript = Normal.Button.GetComponent<GunButton>();
            Hard.ButtonScript = Hard.Button.GetComponent<GunButton>();
        }

        [HarmonyPatch(typeof(LaunchPanel), "OnEnable", new Type[0])]
        private static class SetActiveButtons
        {
            private static void Postfix(LaunchPanel __instance)
            {
                var songData = SongDataHolder.I.songData;

                //set button visibility based on availible diffs
                Easy.Button.GetComponent<MeshRenderer>().enabled = songData.hasEasy;
                Expert.Button.GetComponent<MeshRenderer>().enabled = songData.hasExpert;
                Hard.Button.GetComponent<MeshRenderer>().enabled = songData.hasHard;
                Normal.Button.GetComponent<MeshRenderer>().enabled = songData.hasNormal;

                //set interactivity based onb avalible diffs
                Easy.ButtonScript.SetInteractable(songData.hasEasy);
                Expert.ButtonScript.SetInteractable(songData.hasExpert);
                Hard.ButtonScript.SetInteractable(songData.hasHard);
                Normal.ButtonScript.SetInteractable(songData.hasNormal);
            }
        }

    }
}
