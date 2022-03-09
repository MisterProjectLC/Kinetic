using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
// IngredientDrawerUIE
[CustomPropertyDrawer(typeof(OptionData))]
public class OptionDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 75;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // SETUP ------------------------------------------
        OptionData.Components selected = (OptionData.Components)property.FindPropertyRelative("selected").intValue;
        Hermes.Properties hermesProperty = (Hermes.Properties)property.FindPropertyRelative("property").intValue;

        void AddMenuItemForComponent(GenericMenu menu, string menuPath, OptionData.Components component)
        {
            menu.AddItem(new GUIContent(menuPath), selected.Equals(component), OnComponentSelected, component);
        }

        void OnComponentSelected(object component)
        {
            property.FindPropertyRelative("selected").intValue = (int)component;
            property.serializedObject.ApplyModifiedProperties();
        }

        void AddMenuItemForProperty(GenericMenu menu, string menuPath, Hermes.Properties component)
        {
            menu.AddItem(new GUIContent(menuPath), hermesProperty.Equals(component), OnPropertySelected, component);
        }

        void OnPropertySelected(object component)
        {
            property.FindPropertyRelative("property").intValue = (int)component;
            property.serializedObject.ApplyModifiedProperties();
        }

        // Calculate rects
        var objectRect = new Rect(position.x, position.y, position.width/2, 18);
        var firstDropRect = new Rect(position.x + 2*position.width/3, position.y, position.width/3, position.height/2);
        var secondDropRect = new Rect(position.x, position.y + 0.9f*position.height/3, position.width/2, position.height/3);
        var defaultValueRect = new Rect(position.x, position.y + 1.8f*position.height/3, position.width/2, position.height/4);

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // COMPONENT ----------------------------------------------
        if (EditorGUI.DropdownButton(firstDropRect, new GUIContent(selected.ToString()), FocusType.Passive))
        {
            // create the menu and add items to it
            GenericMenu menu = new GenericMenu();

            // forward slashes nest menu items under submenus
            for (OptionData.Components i = OptionData.Components._First + 1; i < OptionData.Components._Last; i++)
                AddMenuItemForComponent(menu, i.ToString(), i);

            menu.ShowAsContext();
        }

        switch (selected)
        {
            case OptionData.Components.Slider:
                property.FindPropertyRelative("slider").objectReferenceValue = 
                    EditorGUI.ObjectField(objectRect, property.FindPropertyRelative("slider").objectReferenceValue, 
                    typeof(Slider), true);
                break;

            case OptionData.Components.Toggle:
                property.FindPropertyRelative("toggle").objectReferenceValue =
                    EditorGUI.ObjectField(objectRect, property.FindPropertyRelative("toggle").objectReferenceValue, 
                    typeof(Toggle), true);
                break;

            case OptionData.Components.Dropdown:
                property.FindPropertyRelative("dropdown").objectReferenceValue =
                    EditorGUI.ObjectField(objectRect, property.FindPropertyRelative("dropdown").objectReferenceValue, 
                    typeof(Dropdown), true);
                break;
        }

        // PROPERTY -----------------------------------------------
        if (EditorGUI.DropdownButton(secondDropRect, new GUIContent(hermesProperty.ToString()), FocusType.Passive))
        {
            // create the menu and add items to it
            GenericMenu menu = new GenericMenu();

            // forward slashes nest menu items under submenus
            for (Hermes.Properties i = Hermes.Properties._First + 1; i < Hermes.Properties._Last; i++)
                AddMenuItemForProperty(menu, i.ToString(), i);

            menu.ShowAsContext();
        }

        // DEFAULT VALUE --------------------------------------------------------------
        switch (selected)
        {
            case OptionData.Components.Slider:
                property.FindPropertyRelative("defaultFloat").floatValue =
                    EditorGUI.FloatField(defaultValueRect, property.FindPropertyRelative("defaultFloat").floatValue);
                break;

            case OptionData.Components.Toggle:
                property.FindPropertyRelative("defaultBool").boolValue =
                    EditorGUI.Toggle(defaultValueRect, property.FindPropertyRelative("defaultBool").boolValue);
                break;

            case OptionData.Components.Dropdown:
                property.FindPropertyRelative("defaultInt").intValue =
                    EditorGUI.IntField(defaultValueRect, property.FindPropertyRelative("defaultInt").intValue);
                break;
        }

        EditorGUI.EndProperty();
    }
}
#endif