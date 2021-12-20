using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class LanguageDropdown
{
    public LocalizationSystem.Language lang;

    public LanguageDropdown(LocalizationSystem.Language lang)
    {
        this.lang = lang;
    }

    public void RenderDropdown()
    {
        if (EditorGUILayout.DropdownButton(new GUIContent(lang.ToString()), FocusType.Passive, GUILayout.Width(100)))
        {
            // create the menu and add items to it
            GenericMenu menu = new GenericMenu();

            // forward slashes nest menu items under submenus
            for (LocalizationSystem.Language i = LocalizationSystem.Language._First + 1; i < LocalizationSystem.Language._Last; i++)
                AddMenuItemForLanguage(menu, i.ToString(), i);

            menu.ShowAsContext();
        }
    }

    void OnLanguageSelected(object lang)
    {
        this.lang = (LocalizationSystem.Language)lang;
    }

    void AddMenuItemForLanguage(GenericMenu menu, string menuPath, LocalizationSystem.Language lang)
    {
        menu.AddItem(new GUIContent(menuPath), this.lang.Equals(lang), OnLanguageSelected, lang);
    }
}
#endif