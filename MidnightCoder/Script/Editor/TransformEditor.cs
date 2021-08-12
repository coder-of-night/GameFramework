using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    static bool bFastPut = false;
    //static string strChildName = "";
    public override void OnInspectorGUI()
    {
        Transform trans = target as Transform;
        EditorGUIUtility.labelWidth = 15;
        Vector3 pos;
        Vector3 rot;
        Vector3 scale;

        EditorGUILayout.BeginHorizontal();
        Color c = GUI.color;
        GUI.color = Color.green;
        //EditorGUILayout.LabelField("Tris " + GetTotalTri(trans.gameObject));
        GUI.color = c;
        EditorGUILayout.EndHorizontal();
        // Local Position
        EditorGUILayout.BeginHorizontal();
        {
            if (DrawButton("LP", "Reset Local Position", IsResetPositionValid(trans), 26f))
            {
                Undo.RegisterCompleteObjectUndo(trans, "Reset Local Position");
                trans.localPosition = Vector3.zero;
            }
            pos = DrawVector3(trans.localPosition);
        }
        EditorGUILayout.EndHorizontal();

        // Local Rotation
        EditorGUILayout.BeginHorizontal();
        {
            if (DrawButton("LR", "Reset Local Rotation", IsResetRotationValid(trans), 26f))
            {
                Undo.RegisterCompleteObjectUndo(trans, "Reset Local Rotation");
                trans.localEulerAngles = Vector3.zero;
            }
            rot = DrawVector3(trans.localEulerAngles);
        }
        EditorGUILayout.EndHorizontal();

        // Local Scale
        EditorGUILayout.BeginHorizontal();
        {
            if (DrawButton("LS", "Reset Local Scale", IsResetScaleValid(trans), 26f))
            {
                Undo.RegisterCompleteObjectUndo(trans, "Reset Local Scale");
                trans.localScale = new Vector3(1, 1, 1);
            }
            scale = DrawVector3(trans.localScale);
        }
        EditorGUILayout.EndHorizontal();

        // World Position
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("WP");
            DrawVector3(trans.position);
        }
        EditorGUILayout.EndHorizontal();

        // 如果有数值更改，设置 transform 数值
        if (GUI.changed)
        {
            Undo.RegisterCompleteObjectUndo(trans, "Transform Change");
            trans.localPosition = Validate(pos);
            trans.localEulerAngles = Validate(rot);
            trans.localScale = Validate(scale);
        }

        // Copy
        EditorGUILayout.BeginHorizontal();
        {
            //  bFastPut = EditorGUILayout.Toggle(new GUIContent("", "Mouse Put"), bFastPut, EditorStyles.miniButton, GUILayout.Width(20));
            c = GUI.color;
            GUI.color = new Color(1f, 1f, 0.5f, 1f);
            //GUILayout.Button(, GUILayout.Width(width));
            if (GUILayout.Button("Copy Local", EditorStyles.miniButtonLeft))
            {
                v3Pos = trans.localPosition;
                qRotate = trans.localRotation;
                v3Scale = trans.localScale;
            }

            GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
            if (GUILayout.Button("Paste Local", EditorStyles.miniButtonRight))
            {
                Undo.RegisterCompleteObjectUndo(trans, "Paste Local");
                trans.localPosition = v3Pos;
                trans.localRotation = qRotate;
                trans.localScale = v3Scale;
            }


            if (GUILayout.Button("Copy World", EditorStyles.miniButtonLeft))
            {
                v3Pos = trans.position;
                qRotate = trans.rotation;
                v3Scale = trans.localScale;
            }

            GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
            if (GUILayout.Button("Paste World", EditorStyles.miniButtonRight))
            {
                Undo.RegisterCompleteObjectUndo(trans, "Paste Local");
                trans.position = v3Pos;
                trans.rotation = qRotate;
                trans.localScale = v3Scale;
            }

            GUI.color = c;
        }

        EditorGUILayout.EndHorizontal();

        // Copy
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Paste Pos", EditorStyles.miniButtonLeft))
            {
                Undo.RegisterCompleteObjectUndo(trans, "Paste Pos");
                trans.localPosition = v3Pos;
            }

            if (GUILayout.Button("Paste Rot", EditorStyles.miniButtonMid))
            {
                Undo.RegisterCompleteObjectUndo(trans, "Paste Rot");
                trans.localRotation = qRotate;
            }

            if (GUILayout.Button("Paste Sca", EditorStyles.miniButtonRight))
            {
                Undo.RegisterCompleteObjectUndo(trans, "Paste Sca");
                trans.localScale = v3Scale;
            }
        }

        EditorGUILayout.EndHorizontal();

        ////paste ui
        //EditorGUILayout.BeginHorizontal();
        //{
        //    if (GUILayout.Button("Paste UIPosX", EditorStyles.miniButtonLeft))
        //    {
        //        System.Type T = typeof(GUIUtility);
        //        PropertyInfo systemCopyBufferProperty = T.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
        //        string value = (string)systemCopyBufferProperty.GetValue(T, null);

        //        Vector3 localPos = trans.localPosition;

        //        trans.localPosition = new Vector3(float.Parse(value) - 960 / 2, localPos.y, localPos.z);
        //    }

        //    if (GUILayout.Button("Paste UIPosY", EditorStyles.miniButtonMid))
        //    {
        //        System.Type T = typeof(GUIUtility);
        //        PropertyInfo systemCopyBufferProperty = T.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
        //        string value = (string)systemCopyBufferProperty.GetValue(T, null);

        //        Vector3 localPos = trans.localPosition;

        //        trans.localPosition = new Vector3(localPos.x, 640 / 2 - float.Parse(value), localPos.z);
        //    }
        //}

        //EditorGUILayout.EndHorizontal();

        //// AddChild
        //EditorGUILayout.BeginHorizontal();
        //{
        //    if (GUILayout.Button("Add Child", EditorStyles.miniButton, GUILayout.Width(80)))
        //    {
        //        SelectionTools.CreateChild(strChildName);
        //    }

        //    EditorGUILayout.LabelField("Name", GUILayout.Width(40));
        //    strChildName = EditorGUILayout.TextField(strChildName);

        //}
        //EditorGUILayout.EndHorizontal();
    }

    void OnSceneGUI()
    {
        if (!bFastPut)
        {
            return;
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && (Event.current.control || Event.current.alt))
        {
            //create a ray to get where we clicked on terrain/ground/objects in scene view and pass in mouse position
            Transform trans = target as Transform;
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;

            //ray hit something
            if (Physics.Raycast(worldRay, out hitInfo))
            {
                //call this method when you've used an event.
                //the event's type will be set to EventType.Used,
                //causing other GUI elements to ignore it
                Event.current.Use();

                //place a waypoint at clicked point
                Undo.RegisterCompleteObjectUndo(trans, "Hit");
                trans.position = hitInfo.point;
            }
        }
    }

    static Vector3 v3Pos = Vector3.zero;
    static Quaternion qRotate = Quaternion.identity;
    static Vector3 v3Scale = Vector3.one;
    static bool DrawButton(string title, string tooltip, bool enabled, float width)
    {
        if (enabled)
        {
            Color color = GUI.color;
            GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
            bool hexart = GUILayout.Button(new GUIContent(title, tooltip), EditorStyles.miniButton, GUILayout.Width(width));
            GUI.color = color;
            return hexart;
        }
        else
        {
            Color color = GUI.color;
            GUI.color = new Color(1f, 0.5f, 0.5f, 0.25f);
            GUILayout.Button(new GUIContent(title, tooltip), EditorStyles.miniButton, GUILayout.Width(width));
            GUI.color = color;
            return false;
        }
    }

    static Vector3 DrawVector3(Vector3 value)
    {
        GUILayoutOption opt = GUILayout.MinWidth(30f);
        Color color = GUI.color;
        GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
        value.x = EditorGUILayout.FloatField("X", value.x, opt);
        GUI.color = new Color(0.5f, 1f, 0.5f, 1f);
        value.y = EditorGUILayout.FloatField("Y", value.y, opt);
        GUI.color = new Color(0.5f, 0.75f, 1f, 1f);
        value.z = EditorGUILayout.FloatField("Z", value.z, opt);
        GUI.color = color;
        return value;
    }

    static bool IsResetPositionValid(Transform targetTransform)
    {
        Vector3 v = targetTransform.localPosition;
        return (v.x != 0f || v.y != 0f || v.z != 0f);
    }

    static bool IsResetRotationValid(Transform targetTransform)
    {
        Vector3 v = targetTransform.localEulerAngles;
        return (v.x != 0f || v.y != 0f || v.z != 0f);
    }

    static bool IsResetScaleValid(Transform targetTransform)
    {
        Vector3 v = targetTransform.localScale;
        return (v.x != 1f || v.y != 1f || v.z != 1f);
    }

    static Vector3 Validate(Vector3 vector)
    {
        vector.x = float.IsNaN(vector.x) ? 0f : vector.x;
        vector.y = float.IsNaN(vector.y) ? 0f : vector.y;
        vector.z = float.IsNaN(vector.z) ? 0f : vector.z;
        return vector;
    }

    //static string GetTotalTri(GameObject obj)
    //{
    //    float count = 0;
    //    MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
    //    foreach (MeshFilter meshFilter in meshFilters)
    //    {
    //        count += meshFilter.sharedMesh.triangles.Length / 3;
    //    }

    //    SkinnedMeshRenderer[] skinnedMeshRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
    //    foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
    //    {
    //        count += skinnedMeshRenderer.sharedMesh.triangles.Length / 3;
    //    }
    //    //Terrain[] terrains = obj.GetComponentsInChildren<Terrain>();

    //    //foreach(Terrain terrain in terrains)
    //    //{
    //    //    count += terrain.terrainData.size.x * terrain.terrainData.size.z *1.0f* (terrain.terrainData.heightmapResolution-1) / 16 / 16 * 2/3;
    //    //}
    //    return Global.KFormat(count);

    //}
}

public class SelectionTools
{
    #region Helper Functions

    class AssetEntry
    {
        public string path;
        public List<System.Type> types = new List<System.Type>();
    }

    static bool HasValidSelection()
    {
        if (Selection.objects == null || Selection.objects.Length == 0)
        {
            Debug.LogWarning("You must select an object first");
            return false;
        }
        return true;
    }

    static bool HasValidTransform()
    {
        if (Selection.activeTransform == null)
        {
            Debug.LogWarning("You must select an object first");
            return false;
        }
        return true;
    }

    static List<AssetEntry> GetDependencyList(Object[] objects)
    {
        Object[] deps = EditorUtility.CollectDependencies(objects);

        List<AssetEntry> list = new List<AssetEntry>();

        foreach (Object obj in deps)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (!string.IsNullOrEmpty(path))
            {
                bool found = false;
                System.Type type = obj.GetType();

                foreach (AssetEntry ent in list)
                {
                    if (string.Equals(ent.path, path))
                    {
                        if (!ent.types.Contains(type)) ent.types.Add(type);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    AssetEntry ent = new AssetEntry();
                    ent.path = path;
                    ent.types.Add(type);
                    list.Add(ent);
                }
            }
        }

        deps = null;
        objects = null;
        return list;
    }

    static string RemovePrefix(string text)
    {
        text = text.Replace("UnityEngine.", "");
        text = text.Replace("UnityEditor.", "");
        return text;
    }

    static string GetDependencyText(Object[] objects)
    {
        List<AssetEntry> dependencies = GetDependencyList(objects);
        List<string> list = new List<string>();
        string text = "";

        foreach (AssetEntry ae in dependencies)
        {
            text = ae.path.Replace("Assets/", "");

            if (ae.types.Count > 1)
            {
                text += " (" + RemovePrefix(ae.types[0].ToString());

                for (int i = 1; i < ae.types.Count; ++i)
                {
                    text += ", " + RemovePrefix(ae.types[i].ToString());
                }

                text += ")";
            }
            list.Add(text);
        }

        list.Sort();

        text = "";
        foreach (string s in list) text += s + "\n";
        list.Clear();
        list = null;

        dependencies.Clear();
        dependencies = null;
        return text;
    }
    #endregion
}
