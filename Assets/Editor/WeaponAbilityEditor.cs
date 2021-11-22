using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (WeaponAbility))]
public class WeaponAbilityEditor : Editor
{
    SerializedObject so;
    SerializedProperty weaponRef;

    private void OnEnable()
    {
        so = serializedObject;
        weaponRef = so.FindProperty("WeaponRef");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(weaponRef);

        //base.OnInspectorGUI();
    }
}
