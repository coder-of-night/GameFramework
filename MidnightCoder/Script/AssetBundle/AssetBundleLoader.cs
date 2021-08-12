using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
namespace MidnightCoder.Game
{
    public class AssetBundleLoader : MonoBehaviour
    {
        //
        // Static Fields
        //
        private const string PREFS_MANIFEST_VERSION = "ManifestVersion";

        private const string TARGET = "Android";

        //
        // Fields
        //
        private float progress;

        private string statusMsg;

        private bool isDone;

        private string error;

        private int manifestVersion;

        private AssetBundleManifest manifest;

        private string urlRoot;

        //
        // Properties
        //
        public string Error
        {
            get
            {
                return this.error;
            }
        }

        public bool IsDone
        {
            get
            {
                return this.isDone;
            }
        }

        public float Progress
        {
            get
            {
                return this.progress;
            }
        }

        public string StatusMsg
        {
            get
            {
                return this.statusMsg;
            }
        }

        //
        // Methods
        //
        public void Download()
        {
            StartCoroutine("DownloadCR");
        }

        //private IEnumerator DownloadCR() {
        //    urlRoot = AssetBundleConfig.Instance.URLRoot;
        //    while (urlRoot.EndsWith("/"))
        //    {
        //        urlRoot = urlRoot.Substring(0, urlRoot.Length - 1);
        //    }
        //    urlRoot = urlRoot + "/Android";
        //   StartCoroutine("LoadManifestCR");




        //};

        private IEnumerator LoadEachBundleCR;

        private IEnumerator LoadManifestCR;
    }
}