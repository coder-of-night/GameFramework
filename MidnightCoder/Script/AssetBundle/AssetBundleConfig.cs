using System;
using UnityEngine;
namespace MidnightCoder.Game
{
    [CreateAssetMenu(fileName = "Assets/MidnightCoder/Resources/AssetBundleConfig", menuName = "MidnightCoder Config/Asset Bundle Config")]
    public class AssetBundleConfig : ScriptableObject
    {
        //
        // Static Fields
        //
        private static AssetBundleConfig instance;

        //
        // Fields
        //
        public AssetBundleConfig.Type type;

        public bool downloadBundles;

        [TextArea(1, 5)]
        public string urlDevelop = string.Empty;

        [TextArea(1, 5)]
        public string urlStaging = string.Empty;

        [TextArea(1, 5)]
        public string urlProduction = string.Empty;

        //
        // Static Properties
        //
        public static AssetBundleConfig Instance
        {
            get
            {
                if (AssetBundleConfig.instance == null)
                {
                    AssetBundleConfig.instance = (AssetBundleConfig)Resources.Load("AssetBundleConfig");
                }
                return AssetBundleConfig.instance;
            }
        }

        //
        // Properties
        //
        public string URLRoot
        {
            get
            {
                string text;
                if (this.type == AssetBundleConfig.Type.Production)
                {
                    text = this.urlProduction;
                }
                else
                {
                    if (this.type == AssetBundleConfig.Type.Staging)
                    {
                        text = this.urlStaging;
                    }
                    else
                    {
                        text = this.urlDevelop;
                    }
                }
                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }
                text = text.TrimEnd(new char[] {
                '/'
            });
                return string.Format("{0}/{1}/", text, BundleInfo.BuildNumber);
            }
        }

        //
        // Nested Types
        //
        public enum Type
        {
            Develop,
            Staging,
            Production
        }
    }
}