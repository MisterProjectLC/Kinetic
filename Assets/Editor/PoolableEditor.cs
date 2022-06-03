using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Poolable))]
[CanEditMultipleObjects]
public class PoolableEditor : Editor
{
    string enumName;

    public override void OnInspectorGUI()
    {
        Poolable loader = target as Poolable;

        base.OnInspectorGUI();

        enumName = GUILayout.TextField(enumName);

        if (GUILayout.Button("Create new PoolableEnum"))
            CreateMyAsset(enumName);
    }

    public static void CreateMyAsset(string enumName)
    {
        PoolableEnum asset = CreateInstance<PoolableEnum>();

        string name = AssetDatabase.GenerateUniqueAssetPath("Assets/Scripts/Entities/PoolableObjects/Enum/" + enumName + ".asset");
        AssetDatabase.CreateAsset(asset, name);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
#endif