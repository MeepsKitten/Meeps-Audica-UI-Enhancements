using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using MelonLoader;

namespace AudicaModding.MeepsUIEnhancements
{
    class SongTimeUIManager : MonoBehaviour
    {
        public SongTimeUIManager(IntPtr intPtr) : base(intPtr) { }

        public static GameObject scorp;
        public static GameObject panel;

        private static TextMeshProUGUI display;
        void Awake()
        {
            display = GetComponentInChildren<TextMeshProUGUI>();
            MelonLogger.Msg("found display text");
            panel = display.gameObject.transform.parent.GetChild(1).gameObject;
            MelonLogger.Msg($"found panel {panel.name}");          
        }

        void Update()
        {
            if (!AudioDriver.I) return;


            //update display with time
            if (display)
            {
                var seconds = AudioDriver.I.GetSongPositionSeconds(AudioDriver.TickContext.Scoring);
                var curTime = SongProgressDisplay.SecondsToMinSec(seconds);

                var totalLengthInSex = (SongTimeUI.SongLengthMins * 60) + SongTimeUI.SongLengthSex;

                if (seconds <= totalLengthInSex)
                {

                    if (!Config.Config.ShowProgressAsPercentage)
                    {
                        display.text = $"{curTime}/{SongTimeUI.SongLength}";
                    }
                    else
                    {
                        display.text = $"{(int)(seconds / totalLengthInSex * 100)}% Complete";

                    }
                }
                else
                {
                    if (Config.Config.ShowProgressAsPercentage)
                    {
                        display.text = @"\(^-^)/";
                    }
                }
            }

            //look for material to swap into the panel
            if (!scorp)
            {
                scorp = GameObject.Find("ScoreKeeperDisplay/background");
                if (scorp)
                {
                    MelonLogger.Msg($"found scorepanel {scorp.name}");

                    panel.GetComponent<Renderer>().material = scorp.GetComponent<Renderer>().material;
                }
            }
        }
    }
}
