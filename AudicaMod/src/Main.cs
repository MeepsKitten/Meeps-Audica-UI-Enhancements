using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using System.Linq;
using System.IO;
using UnhollowerRuntimeLib;
using AudicaModding.MeepsUIEnhancements;

[assembly: MelonOptionalDependencies(new string[] { "SongDataLoader", "SongBrowser" })]

namespace AudicaModding.MeepsUIEnhancements
{

    public class MeepsUIEnhancements : MelonMod
    {
        public static class BuildInfo
        {
            public const string Name = "MeepsUXEnhancements";  // Name of the Mod.  (MUST BE SET)
            public const string Author = "MeepsKitten"; // Author of the Mod.  (Set as null if none)
            public const string Company = null; // Company that made the Mod.  (Set as null if none)
            public const string Version = "1.2.0"; // Version of the Mod.  (MUST BE SET)
            public const string DownloadLink = "https://github.com/MeepsKitten/Meeps-Audica-UI-Enhancements/releases/latest"; // Download Link for the Mod.  (Set as null if none)
        }

        public static bool songDataLoaderInstalled = false;
        private string minSongDataLoaderVersion = "1.1.0";
        private string problematicSongBrowserVersion = "3.0.3";
        public static Sprite defaultAlbumArt;

        public static bool songBrowserInstalled = false;


        public string[] SongDataLoaderDependants =
        {
            "Album Art Display"
        };

        public override void OnUpdate()
        {
            //WatchController.Update();
        }

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
            ClassInjector.RegisterTypeInIl2Cpp<SongTimeOverlayUIManager>();

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

            if (MelonHandler.Mods.Any(it => (it.Info.SystemType.Name == nameof(SongBrowser) && (Util.VersionCompare.versionCompare(it.Info.Version, problematicSongBrowserVersion) > 0))))
            {
                songBrowserInstalled = true;
                MelonLogger.Warning("Please note that the song progress indicator will not display when using Song Browser's Marathon Mode");
            }
            

            defaultAlbumArt = Sprite.Create(Util.LoadAssets.Texture2DFromAssetBundle("UI Ehancements.src.AlbumArt.defaultart", "song.png"), new Rect(0, 0, 256, 256), Vector2.zero);
            defaultAlbumArt.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(defaultAlbumArt);

           
        }

    }
}



