using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace MidnightCoder.Game
{
    public class Util
    {
        //
        // Static Fields
        //
        private static float worldScreenHeight = -1;

        private static float worldScreenWidth = -1;

        private static float halfHeight = -1;

        private static float halfWidth = -1;

        //
        // Static Methods
        //
        public static T AndroidLibCallStatic<T>(string func)
        {
            try
            {
                AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.midnightcoder.androidlib.Util");
                T result;
                if (androidJavaClass == null)
                {
                    Debug.LogWarning("Android class not found");
                    result = default(T);
                    return result;
                }
                result = androidJavaClass.CallStatic<T>(func, new object[0]);
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Something went wrong when calling " + new object[] {
                    func
                });
                Debug.LogWarning(ex.ToString());
            }
            return default(T);
        }

        public static void AndroidLibCallStatic(string func)
        {
            try
            {
                AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.midnightcoder.androidlib.Util");
                if (androidJavaClass == null)
                {
                    Debug.LogWarning("Android class not found");
                }
                else
                {
                    androidJavaClass.CallStatic(func, new object[0]);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Something went wrong when calling "+ new object[] {
                    func
                });
                Debug.LogWarning(ex.ToString());
            }
        }
        /// <summary>
        /// ??????????????????
        /// </summary>
        private static void ComputeBoundsRecursive(Transform transform, ref Vector3 min, ref Vector3 max)
        {
            if (transform == null)
            {
                return;
            }
            foreach (Transform transform2 in transform)
            {
                if (!(transform2 == null))
                {
                    Util.ComputeBoundsRecursive(transform2, ref min, ref max);
                }
            }
            Renderer component = transform.GetComponent<Renderer>();
            if (component != null)
            {
                Bounds bounds = component.bounds;
                if (bounds.min.x < min.x)
                {
                    min.x = bounds.min.x;
                }
                if (bounds.min.y < min.y)
                {
                    min.y = bounds.min.y;
                }
                if (bounds.min.z < min.z)
                {
                    min.z = bounds.min.z;
                }
                if (bounds.max.x > max.x)
                {
                    max.x = bounds.max.x;
                }
                if (bounds.max.y > max.y)
                {
                    max.y = bounds.max.y;
                }
                if (bounds.max.z > max.z)
                {
                    max.z = bounds.max.z;
                }
            }
        }
        public static void DestroyChildrenWithSubString(GameObject parent, string substring = "", bool immediate = false)
        {
            if (parent == null)
            {
                return;
            }
            List<GameObject> list = new List<GameObject>();
            foreach (Transform transform in parent.transform)
            {
                if (!(transform == null) && !(transform.gameObject == null))
                {
                    string name = transform.gameObject.name;
                    if (substring.Length == 0 || name.IndexOf(substring) != -1)
                    {
                        list.Add(transform.gameObject);
                    }
                }
            }
            foreach (GameObject current in list)
            {
                if (immediate)
                    UnityEngine.Object.DestroyImmediate(current);
                else
                    UnityEngine.Object.Destroy(current);
            }
            list.Clear();
        }
        /// <summary>
        /// ????????????????????????????????????name?????????,????????????????????????
        /// </summary>
        public static bool FindWithFullName(GameObject parent, string name, ref List<GameObject> outSet)
        {
            if (parent == null)
            {
                return false;
            }
            if (parent.name == name)
            {
                outSet.Add(parent);
            }
            int childCount = parent.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (!(child == null))
                {
                    if (!(child.gameObject == null))
                    {
                        Util.FindWithFullName(child.gameObject, name, ref outSet);
                    }
                }
            }
            return false;
        }




        /// <summary>
        /// ????????????????????????????????????????????????name?????????,????????????????????????
        /// </summary>
        public static bool FindWithSubString(GameObject parent, string name, ref List<GameObject> outSet, bool ignoreCase = false)
        {
            if (parent == null)
            {
                return false;
            }
            string parentName = ignoreCase? parent.name.ToLower():parent.name;
            string compareName = ignoreCase ? name.ToLower() : name;
            if (parentName.Contains(compareName))
            {
                outSet.Add(parent);
            }
            int childCount = parent.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (!(child == null))
                {
                    if (!(child.gameObject == null))
                    {
                        Util.FindWithSubString(child.gameObject, name, ref outSet);
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// ??????????????????
        /// </summary>
        public static Bounds GetBounds(GameObject go)
        {
            Bounds result = default(Bounds);
            if (go == null)
            {
                return result;
            }
            Vector3 max = Vector3.negativeInfinity;
            Vector3 min = Vector3.positiveInfinity;
            Util.ComputeBoundsRecursive(go.transform, ref min, ref max);
            result.min = min;
            result.max = max;
            return result;
        }
        /// <summary>
        /// ????????????????????????????????????tag?????????,????????????????????????
        /// </summary>
        public static bool FindWithTag(GameObject parent, string tag, ref List<GameObject> outSet)
        {
            if (parent == null)
            {
                return false;
            }
            if (parent.CompareTag(tag))
            {
                outSet.Add(parent);
            }
            int childCount = parent.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (!(child == null))
                {
                    if (!(child.gameObject == null))
                    {
                        Util.FindWithTag(child.gameObject, tag, ref outSet);
                    }
                }
            }
            return false;
        }

        public static bool IsInMainCamera(Bounds b)
        {
            if (Camera.main == null)
            {
                return false;
            }
            if (Camera.main.transform == null)
            {
                return false;
            }
            if (Util.worldScreenHeight < 0)
            {
                Util.worldScreenHeight = Camera.main.orthographicSize * 2;
                Util.worldScreenWidth = Util.worldScreenHeight / (float)Screen.height * (float)Screen.width;
                Util.halfHeight = Util.worldScreenHeight * 0.5f;
                Util.halfWidth = Util.worldScreenWidth * 0.5f;
            }
            Vector3 position = Camera.main.transform.position;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            bounds.min = new Vector3(position.x - Util.halfWidth, position.y - Util.halfHeight, 0);
            bounds.max = new Vector3(position.x + Util.halfWidth, position.y + Util.halfHeight, 0);
            return bounds.Contains(new Vector3(b.min.x, b.min.y)) || bounds.Contains(new Vector3(b.min.x, b.max.y)) || bounds.Contains(new Vector3(b.max.x, b.min.y)) || bounds.Contains(new Vector3(b.max.x, b.max.y));
        }
        /// <summary>
        /// ???????????????bounds?????????Main Camera???????????????
        /// </summary>
        public static bool IsInMainCamera(GameObject go)
        {
            return Util.IsInMainCamera(Util.GetBounds(go));
        }

        public static string ToRoman(uint number)
        {
            StringBuilder stringBuilder = new StringBuilder();
            number = ((number <= 3999) ? number : 3999);
            while (number > 0)
            {
                if (number >= 1000)
                {
                    number -= 1000;
                    stringBuilder.Append("M");
                }
                if (number >= 900)
                {
                    number -= 900;
                    stringBuilder.Append("CM");
                }
                if (number >= 500)
                {
                    number -= 500;
                    stringBuilder.Append("D");
                }
                if (number >= 400)
                {
                    number -= 400;
                    stringBuilder.Append("CD");
                }
                if (number >= 100)
                {
                    number -= 100;
                    stringBuilder.Append("C");
                }
                if (number >= 90)
                {
                    number -= 90;
                    stringBuilder.Append("XC");
                }
                if (number >= 50)
                {
                    number -= 50;
                    stringBuilder.Append("L");
                }
                if (number >= 40)
                {
                    number -= 40;
                    stringBuilder.Append("XL");
                }
                if (number >= 10)
                {
                    number -= 10;
                    stringBuilder.Append("X");
                }
                if (number >= 9)
                {
                    number -= 9;
                    stringBuilder.Append("IX");
                }
                if (number >= 5)
                {
                    number -= 5;
                    stringBuilder.Append("V");
                }
                if (number >= 4)
                {
                    number -= 4;
                    stringBuilder.Append("IV");
                }
                if (number >= 1)
                {
                    number -= 1;
                    stringBuilder.Append("I");
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        ///?????????arr???????????????1?????????(?????????)
        /// </summary>
        public static void GetRandomElement<T>(T[] arr, ref T item)
        {
            int num = arr.Length;
            if (num > 0)
            {
                int i = UnityEngine.Random.Range(0, num);
                item = arr[i];
            }
        }
        /// <summary>
        ///?????????arr???????????????1?????????(?????????)
        /// </summary>
        public static void GetRandomElement<T>(List<T> arr, ref T item)
        {
            int num = arr.Count;
            if (num > 0)
            {
                int i = UnityEngine.Random.Range(0, num);
                item = arr[i];
            }
        }
        /// <summary>
        ///????????????arr?????????amount???????????????????????????
        /// </summary>
        public static void GetRandomAmountElementUnRepeat<T>(T[] arr, int amount, ref List<int> indexArr)
        {
            int arrLength = arr.Length;
            if (amount >= arrLength)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    indexArr.Add(i);
                }
                return;
            }
            else if (amount <= 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    int temp;
                    do
                    {
                        temp = UnityEngine.Random.Range(0, arrLength);
                    } while (indexArr.Contains(temp));
                    indexArr.Add(temp);
                }
            }
        }
        /// <summary>
        ///????????????arr?????????amount???????????????????????????
        /// </summary>
        public static void GetRandomAmountElementUnRepeat<T>(List<T> arr, int amount, ref List<int> indexArr)
        {
            int arrLength = arr.Count;
            if (amount >= arrLength)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    indexArr.Add(i);
                }
                return;
            }
            else if (amount <= 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    int temp;
                    do
                    {
                        temp = UnityEngine.Random.Range(0, arrLength);
                    } while (indexArr.Contains(temp));
                    indexArr.Add(temp);
                }
            }
        }
        /// <summary>
        /// ????????????point????????????radius?????????????????????????????????radius_inner???????????????
        /// </summary>
        /// <param name="point">????????????</param>
        /// <param name="radius_outer">????????????</param>
        /// <param name="radius_inner">????????????</param>
        /// <returns>????????????</returns>
        public static Vector2 PointOfRandom(Vector2 point, float radius_outer, float radius_inner)
        {
            float x = UnityEngine.Random.Range(point.x - (radius_outer - radius_inner), point.x + (radius_outer - radius_inner));
            float y = UnityEngine.Random.Range(point.y - (radius_outer - radius_inner), point.y + (radius_outer - radius_inner));

            while (!Util.IsInRegion(x - point.x, y - point.y, radius_outer - radius_inner) )
            {
                x = UnityEngine.Random.Range(point.x - (radius_outer - radius_inner), point.x + (radius_outer - radius_inner));
                y = UnityEngine.Random.Range(point.y - (radius_outer - radius_inner), point.y + (radius_outer - radius_inner));
            }

            var p = new Vector2(x, y);
            return p;
        }
        /// <param name="x_off">??????????????????x???????????????</param>
        /// <param name="y_off">??????????????????y???????????????</param>
        /// <param name="distance">???????????????????????????</param>
        /// <returns>???????????????????????????</returns>
        public static bool IsInRegion(float x_off, float y_off, float distance)
        {
            if (x_off * x_off + y_off * y_off <= distance * distance)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// ????????????????????????????????????????????????
        /// </summary>
        /// <param name="p_outer">????????????</param>
        /// <param name="r_outer">????????????</param>
        /// <param name="p_inner">????????????</param>
        /// <param name="r_inner">????????????</param>
        /// <returns>???????????????</returns>
        public static bool IsIntersect(Vector2 p_outer, float r_outer, Vector2 p_inner, float r_inner)
        {
            //????????????????????????????????? + r_inner = r_outer
            float distance = (float)(Mathf.Sqrt((p_outer.x - p_inner.x) * (p_outer.x - p_inner.x) + (p_outer.y - p_inner.y) * (p_outer.y - p_inner.y)));
            if (distance + r_inner >= r_outer)
            {
                return true;
            }
            return false;
        }

    }
}
