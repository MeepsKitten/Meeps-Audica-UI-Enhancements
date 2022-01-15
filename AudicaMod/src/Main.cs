using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using System.Linq;
using System.IO;
using UnhollowerRuntimeLib;
using AudicaModding.MeepsUIEnhancements;

[assembly: MelonOptionalDependencies("SongDataLoader")]

namespace AudicaModding.MeepsUIEnhancements
{

    public class MeepsUIEnhancements : MelonMod
    {
        public static class BuildInfo
        {
            public const string Name = "MeepsUXEnhancements-XionEdit";  // Name of the Mod.  (MUST BE SET)
            public const string Author = "MeepsKitten/XionsProphecy"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "1.2.1"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = "https://github.com/MeepsKitten/Meeps-Audica-UI-Enhancements/releases/latest"; // Download Link for the Mod.  (Set as null if none)
        }

        public static bool songDataLoaderInstalled = false;
        private string minSongDataLoaderVersion = "1.1.0";
        public static Sprite defaultAlbumArt;

        public string[] SongDataLoaderDependants =
        {
            "Album Art Display"
        };

        public override void OnPreferencesSaved()
        {
            Config.Config.OnPreferencesSaved();
        }

        public override void OnApplicationStart()
        {
            ClassInjector.RegisterTypeInIl2Cpp<QuickDifficultyPanelManager>();
            ClassInjector.RegisterTypeInIl2Cpp<AlbumArt.AlbumArtShoot>();
            ClassInjector.RegisterTypeInIl2Cpp<PreviewButtonDataStorer>();
            ClassInjector.RegisterTypeInIl2Cpp<SongTimeUIManager>();

            Config.Config.RegisterConfig();

            if (MelonHandler.Mods.Any(it => (it.Info.SystemType.Name == nameof(SongDataLoader) && (Util.VersionCompare.versionCompare(it.Info.Version, minSongDataLoaderVersion) > 0))))
            {
                songDataLoaderInstalled = true;
                MelonLogger.Msg("Song Data Loader is installed. Enabling integration");
            }
            else
            {
                string textOutput = "Song Data Loader is not installed or up to date. Consider downloading/updating it\nThe following features will not function:";
                foreach (string memo in SongDataLoaderDependants)
                {
                    textOutput += "\n" + memo;
                }
                MelonLogger.Error(textOutput);
            }
         
            defaultAlbumArt = Sprite.Create(Util.LoadAssets.Texture2DFromAssetBundle("UI Ehancements.src.AlbumArt.defaultart", "song.png"), new Rect(0, 0, 256, 256), Vector2.zero);
            defaultAlbumArt.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(defaultAlbumArt);
        }

    }
}



