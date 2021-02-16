using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using System.Linq;

namespace AudicaModding.MeepsUIEnhancements
{
    public class MeepsUIEnhancements : MelonMod
    {
        public static class BuildInfo
        {
            public const string Name = "Meeps' UI Ehancements";  // Name of the Mod.  (MUST BE SET)
            public const string Author = "MeepsKitten"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "1.0.1"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
        }

        public static bool songDataLoaderInstalled = false;
        private string minSongDataLoaderVersion = "1.1.0";

        public string[] SongDataLoaderDependants =
        {
            "Album Art Display"
        };

        public override void OnApplicationStart()
        {
            if (MelonHandler.Mods.Any(it => (it.Info.SystemType.Name == nameof(SongDataLoader) && (Util.VersionCompare.versionCompare(it.Info.Version, minSongDataLoaderVersion) > 0))))
            {
                songDataLoaderInstalled = true;
                MelonLogger.Log("Song Data Loader is installed. Enabling integration");
            }
            else
            {
                string textOutput = "Song Data Loader is not installed or up to date. Consider downloading/updating it\nThe following features will not function:";
                foreach (string memo in SongDataLoaderDependants)
                {
                    textOutput += "\n" + memo;
                }
                MelonLogger.LogError(textOutput);
            }

        }

    }
}



