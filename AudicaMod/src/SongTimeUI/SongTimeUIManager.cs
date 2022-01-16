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

        public GameObject scorp;
        public GameObject panel;

        private TextMeshProUGUI display;
        void Awake()
        {
            display = GetComponentInChildren<TextMeshProUGUI>();
            MeepsLogger.Msg("found display text");

                panel = display.gameObject.transform.parent.GetChild(1).gameObject;
                MeepsLogger.Msg($"found panel {panel.name}");
      
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
                    MeepsLogger.Msg($"found scorepanel {scorp.name}");

                    panel.GetComponent<Renderer>().material = scorp.GetComponent<Renderer>().material;
                }
            }
        }
    }

    class SongTimeOverlayUIManager : MonoBehaviour
    {
        public SongTimeOverlayUIManager(IntPtr intPtr) : base(intPtr) { }

        private TextMeshProUGUI display;
        void Awake()
        {
            display = GetComponentInChildren<TextMeshProUGUI>();
            MeepsLogger.Msg("found display text");
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
          
        }
    }
}
