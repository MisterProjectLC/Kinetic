using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ScenePartLoader))]
public class ScenePartLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ScenePartLoader loader = target as ScenePartLoader;

        base.OnInspectorGUI();

        if (GUILayout.Button("Reset Save"))
            SaveManager.Save(new List<string>(), loader.gameObject.name);
    }
}
#endif