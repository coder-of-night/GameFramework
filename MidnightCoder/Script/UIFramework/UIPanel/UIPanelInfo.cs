using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI对应路径实体类,从json中转换
/// </summary>
[Serializable]//表示可序列化的类,表明此类对象可写入磁盘
public class UIPanelInfo : ISerializationCallbackReceiver {

    [NonSerialized]
    public UIPanelType panelType;//json字符串解析无法直接转换枚举类型,所以需要手动转换

    //用它进行序列化反序列化,因为json为字符串
    public string panelTypeString;

    public string path;

    /// <summary>
    /// 反序列化成功后回调
    /// </summary>
    public void OnAfterDeserialize()
    {
        this.panelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), this.panelTypeString);
    }
    /// <summary>
    /// 序列化前回调
    /// </summary>
    public void OnBeforeSerialize()
    {
        this.panelTypeString = this.panelType.ToString();
    }
}
