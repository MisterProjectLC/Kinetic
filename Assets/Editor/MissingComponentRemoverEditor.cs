using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(MissingComponentRemover))]
public class MissingComponentRemoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MissingComponentRemover loader = target as MissingComponentRemover;

        base.OnInspectorGUI();

        if (GUILayout.Button("Remove Missing Components"))
            loader.Clean(loader.transform);
    }
}
#endif