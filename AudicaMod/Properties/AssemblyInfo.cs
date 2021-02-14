using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using AudicaModding.MeepsUIEnhancements;

[assembly: AssemblyTitle(MeepsUIEnhancements.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(MeepsUIEnhancements.BuildInfo.Company)]
[assembly: AssemblyProduct(MeepsUIEnhancements.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + MeepsUIEnhancements.BuildInfo.Author)]
[assembly: AssemblyTrademark(MeepsUIEnhancements.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(MeepsUIEnhancements.BuildInfo.Version)]
[assembly: AssemblyFileVersion(MeepsUIEnhancements.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(MeepsUIEnhancements), MeepsUIEnhancements.BuildInfo.Name, MeepsUIEnhancements.BuildInfo.Version, MeepsUIEnhancements.BuildInfo.Author, MeepsUIEnhancements.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Harmonix Music Systems, Inc.", "Audica")]