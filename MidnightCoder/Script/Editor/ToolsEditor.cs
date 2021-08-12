using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ToolsEditor : Editor
{
    [MenuItem("Tools/清除存储数据")]
    public static void ClearSavedData()
    {
        PlayerPrefs.DeleteAll();
        Debug.LogWarning("PlayerPrefs数据清除!");
    }
}
