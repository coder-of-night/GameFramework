using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public static class TransformExtensions
{
    public static Vector3 SetPosX(this Transform transform, float value)
    {
        Vector3 newPos = new Vector3(value, transform.position.y, transform.position.z);
        transform.position = newPos;
        return newPos;
    }
    public static Vector3 SetPosY(this Transform transform, float value)
    {
        Vector3 newPos = new Vector3(transform.position.x, value, transform.position.z);
        transform.position = newPos;
        return newPos;
    }
    public static Vector3 SetPosZ(this Transform transform, float value)
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, value);
        transform.position = newPos;
        return newPos;
    }
    public static Vector3 SetLocalPosX(this Transform transform, float value)
    {
        Vector3 newPos = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = newPos;
        return newPos;
    }
    public static Vector3 SetLocalPosY(this Transform transform, float value)
    {
        Vector3 newPos = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
        transform.localPosition = newPos;
        return newPos;
    }
    public static Vector3 SetLocalPosZ(this Transform transform, float value)
    {
        Vector3 newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
        transform.localPosition = newPos;
        return newPos;
    }
    public static Vector3 SetEulerX(this Transform transform, float value)
    {
        Vector3 newEuler = new Vector3(value, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.eulerAngles = newEuler;
        return newEuler;
    }
    public static Vector3 SetEulerY(this Transform transform, float value)
    {
        Vector3 newEuler = new Vector3(transform.eulerAngles.x, value, transform.eulerAngles.z);
        transform.eulerAngles = newEuler;
        return newEuler;
    }
    public static Vector3 SetEulerZ(this Transform transform, float value)
    {
        Vector3 newEuler = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, value);
        transform.eulerAngles = newEuler;
        return newEuler;
    }
    public static Vector3 SetLocalEulerX(this Transform transform, float value)
    {
        Vector3 newEuler = new Vector3(value, transform.localEulerAngles.y, transform.localEulerAngles.z);
        transform.localEulerAngles = newEuler;
        return newEuler;
    }
    public static Vector3 SetLocalEulerY(this Transform transform, float value)
    {
        Vector3 newEuler = new Vector3(transform.localEulerAngles.x, value, transform.localEulerAngles.z);
        transform.localEulerAngles = newEuler;
        return newEuler;
    }
    public static Vector3 SetLocalEulerZ(this Transform transform, float value)
    {
        Vector3 newEuler = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, value);
        transform.localEulerAngles = newEuler;
        return newEuler;
    }
    public static Vector3 SetLossyScaleX(this Transform transform, float value)
    {
        Vector3 newScale = new Vector3(value, transform.lossyScale.y, transform.lossyScale.z);
        transform.localScale = newScale;
        return newScale;
    }
    public static Vector3 SetLossyScaleY(this Transform transform, float value)
    {
        Vector3 newScale = new Vector3(transform.lossyScale.x, value, transform.lossyScale.z);
        transform.localScale = newScale;
        return newScale;
    }
    public static Vector3 SetLossyScaleZ(this Transform transform, float value)
    {
        Vector3 newScale = new Vector3(transform.lossyScale.x, transform.lossyScale.y, value);
        transform.localScale = newScale;
        return newScale;
    }
    public static Vector3 SetLocalScaleX(this Transform transform, float value)
    {
        Vector3 newScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
        transform.localScale = newScale;
        return newScale;
    }
    public static Vector3 SetLocalScaleY(this Transform transform, float value)
    {
        Vector3 newScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
        transform.localScale = newScale;
        return newScale;
    }
    public static Vector3 SetLocalScaleZ(this Transform transform, float value)
    {
        Vector3 newScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
        transform.localScale = newScale;
        return newScale;
    }
    public static RectTransform rectTransform(this MonoBehaviour script)
    {
        return script.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Tip:只查一级
    /// </summary>
    public static GameObject FindWithSubString(this Transform transform, string name, bool ignoreCase = false)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                string childName = ignoreCase ? child.name.ToLower() : transform.name;
                string compareName = ignoreCase ? name.ToLower() : name;
                if (childName.Contains(compareName))
                {
                    return child.gameObject;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// Tip:只查一级
    /// </summary>
    public static List<GameObject> FindsWithSubString(this Transform transform, string name, bool ignoreCase = false)
    {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                string childName = ignoreCase ? child.name.ToLower() : transform.name;
                string compareName = ignoreCase ? name.ToLower() : name;
                if (childName.Contains(compareName))
                {
                    objs.Add(child.gameObject);
                }
            }
        }
        return objs;
    }
}


public static class DictionaryExtensions
{
    public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
    {
        TValue value = default(TValue);
        dict.TryGetValue(key, out value);
        return value;
    }
    public static TKey GetKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
    {
        TKey key = default(TKey);
        foreach (KeyValuePair<TKey, TValue> kvp in dict)
        {
            if (kvp.Value.Equals(value))
            {
                key = kvp.Key;
            }
        }
        return key;
    }

    /// <summary>
    /// 可覆盖现有值
    /// </summary>
    /// <returns>如果存在Key就返回true，并且覆盖其值</returns>
    public static bool AddAndCover<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,TValue value)
    {
        bool flag = dict.ContainsKey(key);
        if (flag)
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }
        return flag;
    }

}
