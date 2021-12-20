using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
// IngredientDrawerUIE
[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringDrawer : PropertyDrawer
{
    bool dropdown;
    LanguageDropdown langDrop = new LanguageDropdown(LocalizationSystem.Language.English);
    float height;


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (dropdown)
            return height + 25;
        return 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Draw text area
        position.width -= 40;
        position.height = 18;
        Rect valueRect = new Rect(position);
        valueRect.x += 15;
        valueRect.width -= 15;
        SerializedProperty key = property.FindPropertyRelative("key");
        key.stringValue = EditorGUI.TextField(position, key.stringValue);

        // Draw Fold button
        Rect foldButtonRect = new Rect(position);
        foldButtonRect.width = 15;
        dropdown = EditorGUI.Foldout(foldButtonRect, dropdown, "");

        // Draw Search button
        position.x += position.width + 2;
        position.width = 20;
        position.height = 20;
        if (GUI.Button(position, new GUIContent((Texture)Resources.Load("search"))))
            TextLocalizerSearchWindow.Open(key.stringValue, langDrop.lang);

        // Draw Add button
        position.x += position.width + 2;
        if (GUI.Button(position, new GUIContent((Texture)Resources.Load("add"))))
            TextLocalizerEditWindow.Open(key.stringValue, langDrop.lang);


        // Draw Value if Dropdown is active
        if (dropdown)
        {
            string value = LocalizationSystem.GetLocalizedText(key.stringValue, langDrop.lang);
            valueRect.height = GUI.skin.box.CalcHeight(new GUIContent(value), valueRect.width);
            valueRect.y += 21;
            height = valueRect.height;

            EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);

            langDrop.RenderDropdown();
        }

        EditorGUI.EndProperty();
    }
}
#endif