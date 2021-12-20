using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TextLocalizerSearchWindow : EditorWindow
{
    public string search;
    Dictionary<string, string> dict;
    LanguageDropdown langDrop;


    public static void Open(string key, LocalizationSystem.Language lang)
    {
        TextLocalizerSearchWindow window = new TextLocalizerSearchWindow();
        window.titleContent = new GUIContent("Localization Search");

        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Rect rect = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
        window.ShowAsDropDown(rect, new Vector2(500, 500));
        window.search = key;
        window.langDrop = new LanguageDropdown(lang);
    }

    private void OnEnable()
    {
        dict = LocalizationSystem.GetLanguageDict(langDrop.lang);
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            search = EditorGUILayout.TextField("Search: ", search);
            langDrop.RenderDropdown();
        }
        EditorGUILayout.EndHorizontal();

        dict = LocalizationSystem.GetLanguageDict(langDrop.lang);
        foreach (KeyValuePair<string, string> pair in dict)
        {
            if (pair.Key.ToLower().Contains(search.ToLower()) || pair.Value.ToLower().Contains(search.ToLower()))
                EditorGUILayout.LabelField(pair.Key + ": " + pair.Value, GUILayout.MinHeight(50), GUILayout.MaxHeight(200));
        }
    }
}


public class TextLocalizerEditWindow : EditorWindow
{
    public static void Open(string key, LocalizationSystem.Language lang)
    {
        TextLocalizerEditWindow window = new TextLocalizerEditWindow();
        window.titleContent = new GUIContent("Localizer Window");
        window.ShowUtility();
        window.key = key;
        window.langDrop = new LanguageDropdown(lang);
        window.lastLang = lang;
        window.value = LocalizationSystem.GetLocalizedText(key, lang);
    }

    public string key;
    LanguageDropdown langDrop;
    LocalizationSystem.Language lastLang;
    public string value;


    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Key: ", GUILayout.MaxWidth(50));
            key = EditorGUILayout.TextField(key, GUILayout.Width(250), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Lang: ", GUILayout.MaxWidth(50));
            langDrop.RenderDropdown();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Value: ", GUILayout.MaxWidth(50));
            EditorStyles.textArea.wordWrap = true;

            if (lastLang != langDrop.lang)
            {
                lastLang = langDrop.lang;
                value = LocalizationSystem.GetLocalizedText(key, langDrop.lang);
            }

            value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add"))
        {
            if (LocalizationSystem.GetLocalizedText(key, langDrop.lang) != "")
                LocalizationSystem.Replace(key, (int)langDrop.lang, value);
            else
                LocalizationSystem.Add(key, (int)langDrop.lang, value);
        }

        minSize = new Vector2(460, 520);
        maxSize = minSize;
    }
}
