using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 对Dictionary字典容器的扩展
/// 扩展类必须是静态类
/// 扩展方法必须是静态方法
/// </summary>
public static class DictionaryExtension {
    /// <summary>
    /// 尝试根据key得到value
    /// </summary>
    /// <param name="dict">要操作的字典(成员运算符传入)(此参数的类型决定只有对应类型对象才可调用)</param>
    /// <param name="key">键</param>
    /// <returns>得到了返回value,没有得到返回null </returns>
    public static Tvalue TryGet<Tkey,Tvalue>(this Dictionary<Tkey,Tvalue> dict, Tkey key)
    {
        Tvalue value = default(Tvalue);
        dict.TryGetValue(key, out value);
        return value;
    }

}
