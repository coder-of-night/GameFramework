using System;
using UnityEngine;
namespace MidnightCoder.Game
{
    public class BundleInfo : ScriptableObject
    {
        //
        // Static Fields
        //
        private static BundleInfo instance;

        //
        // Fields
        //
        public string bundleVersion;

        public string bundleID;

        //
        // Static Properties
        //
        public static string BuildNumber
        {
            get
            {
                string[] array = BundleInfo.Instance.bundleVersion.Split(new char[] {
                '.'
            });
                if (array.Length >= 3)
                {
                    return array[2];
                }
                return "0";
            }
        }

        public static BundleInfo Instance
        {
            get
            {
                if (null == BundleInfo.instance)
                {
                    BundleInfo.instance = Resources.Load<BundleInfo>("BundleInfo");
                }
                return BundleInfo.instance;
            }
        }

        public static string MajorMinorVersion
        {
            get
            {
                string[] array = BundleInfo.Instance.bundleVersion.Split(new char[] {
                '.'
            });
                if (array.Length >= 2)
                {
                    return string.Format("{0}.{1}", array[0], array[1]);
                }
                if (array.Length == 1)
                {
                    return string.Format("{0}.0", array[0]);
                }
                return "0.0";
            }
        }

        public static string Version
        {
            get
            {
                return BundleInfo.Instance.bundleVersion;
            }
        }

        //
        // Static Methods
        //
        public static void SetVersion(string major, string minor, string buildNumber)
        {
            BundleInfo.Instance.bundleVersion = string.Format("{0}.{1}.{2}", major, minor, buildNumber);
        }
    }
}