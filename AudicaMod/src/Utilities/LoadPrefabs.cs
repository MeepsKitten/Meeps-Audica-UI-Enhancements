using System.IO;
using UnityEngine;

namespace AudicaModding.MeepsUIEnhancements.Util
{
    class LoadAssets
    {
        public static GameObject ObjectFromAssetBundle(string assemblyResourceName, string bundledObjectName)
        {
            var bundle = LoadAssetData(assemblyResourceName);
            var prefab = GameObject.Instantiate<GameObject>(bundle.LoadAsset(bundledObjectName).Cast<GameObject>());

            prefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(prefab);

            return prefab;
        }

        public static GameObject ChildObjectFromAssetBundle(string assemblyResourceName, string bundledObjectName, Transform parent)
        {
            var bundle = LoadAssetData(assemblyResourceName);
            var prefab = GameObject.Instantiate<GameObject>(bundle.LoadAsset(bundledObjectName).Cast<GameObject>(),parent);

            prefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(prefab);

            return prefab;
        }

        public static Texture2D Texture2DFromAssetBundle(string assemblyResourceName, string bundledObjectName)
        {
            var bundle = LoadAssetData(assemblyResourceName);
            var tex = bundle.LoadAsset(bundledObjectName).Cast<Texture2D>();

            tex.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(tex);

            return tex;
        }

        public static Il2CppAssetBundle LoadAssetData(string assemblyResourceName)
        {
            var stream = typeof(LoadAssets).Assembly.GetManifestResourceStream(assemblyResourceName);

            byte[] data;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                data = ms.ToArray();
            }

            return Il2CppAssetBundleManager.LoadFromMemory(data);
        }

        public static void PrintResourceNamesToLog()
        {
            foreach(string name in typeof(LoadAssets).Assembly.GetManifestResourceNames())
            {
                MelonLoader.MelonLogger.Msg(name);
            }
        }
    }
}
