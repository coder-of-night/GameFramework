using UnityEngine;
using UnityEditor;



/// <summary>
/// 定义对带有 `CustomLabelAttribute` 特性的字段的面板内容的绘制行为。
/// </summary>
[CustomPropertyDrawer(typeof(TitleAttribute))]
public class TitleEditor : PropertyDrawer
{
    private GUIContent _label = null;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (_label == null)
        {
            string name = (attribute as TitleAttribute).name;
            _label = new GUIContent(name);
        }

        EditorGUI.PropertyField(position, property, _label);
    }
}

