using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using System.Linq;
using System.IO;
using UnhollowerRuntimeLib;
using AudicaModding.MeepsUIEnhancements.QuickDifficultySelect;
using AudicaModding.MeepsUIEnhancements;

[assembly: MelonOptionalDependencies("SongDataLoader")]

namespace AudicaModding.MeepsUIEnhancements
{

    public class MeepsUIEnhancements : MelonMod
    {
        public static class BuildInfo
        {
            public const string Name = "MeepsUIEnhancements";  // Name of the Mod.  (MUST BE SET)
            public const string Author = "MeepsKitten"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "1.0.2"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
        }

        public static bool songDataLoaderInstalled = false;
        private string minSongDataLoaderVersion = "1.1.0";
        public static Sprite defaultAlbumArt;

        public string[] SongDataLoaderDependants =
        {
            "Album Art Display"
        };

        public override void OnModSettingsApplied()
        {
            Config.Config.OnModSettingsApplied();
        }

        public override void OnApplicationStart()
        {
           Config.Config.RegisterConfig();

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

            ClassInjector.RegisterTypeInIl2Cpp<QuickDifficultyPanelManager>();
            ClassInjector.RegisterTypeInIl2Cpp<AlbumArt.AlbumArtShoot>();
            Util.LoadAssets.PrintResourceNamesToLog();

            defaultAlbumArt = Sprite.Create(Util.LoadAssets.Texture2DFromAssetBundle("UI Ehancements.src.AlbumArt.defaultart", "song.png"), new Rect(0, 0, 256, 256), Vector2.zero);
            defaultAlbumArt.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(defaultAlbumArt);
        }

    }
}



