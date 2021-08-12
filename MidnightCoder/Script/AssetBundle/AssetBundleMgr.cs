using MidnightCoder;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MidnightCoder.Game
{
    public static class AssetBundleMgr
    {
        //
        // Static Fields
        //
        public static Dictionary<string, AssetBundle> set = new Dictionary<string, AssetBundle>();

        public static AssetBundleLoader loader = null;

        //
        // Static Methods
        //
        public static bool IsDownloadDone()
        {
            return AssetBundleMgr.loader == null || AssetBundleMgr.loader.IsDone;
        }

        public static T Load<T>(string path, bool isFullPath = false) where T : UnityEngine.Object
        {
            if (!isFullPath)
            {
                path = string.Format("Assets/Resources/Data/{0}.asset", path);
            }
            AssetBundle assetBundle;
            if (AssetBundleMgr.set.TryGetValue("data", out assetBundle))
            {
                if (assetBundle.Contains(path))
                {
                    return assetBundle.LoadAsset<T>(path);
                }

                Logger.Log("WARN", path + " cannot be found", new object[0]);
            }
            path = path.Replace("Assets/Resources/", string.Empty);
            path = path.Replace(".asset", string.Empty);
            path = path.Replace(".csv", string.Empty);
            return Resources.Load<T>(path);
        }

        public static T[] LoadAll<T>() where T : UnityEngine.Object
        {
            AssetBundle assetBundle;
            if (AssetBundleMgr.set.TryGetValue("data", out assetBundle))
            {
                return assetBundle.LoadAllAssets<T>();
            }
            return Resources.LoadAll<T>(string.Empty);
        }

        public static void StartDownload()
        {
            GameObject gameObject = new GameObject("[AssetBundleLoader]");
            if (gameObject == null)
            {
                Logger.Log("WARN", "Could not create [AssetBundleLoader]", new object[0]);
                return;
            }
            AssetBundleMgr.loader = gameObject.AddComponent<AssetBundleLoader>();
            if (AssetBundleMgr.loader == null)
            {
                Logger.Log("WARN", "Could not create AssetBundleLoader", new object[0]);
                UnityEngine.Object.Destroy(gameObject);
                return;
            }
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            AssetBundleMgr.loader.Download();
        }

        public static void UnloadAll()
        {
            foreach (AssetBundle current in AssetBundleMgr.set.Values)
            {
                if (!(current == null))
                {
                    current.Unload(true);
                }
            }
            AssetBundleMgr.set.Clear();
        }
    }
}