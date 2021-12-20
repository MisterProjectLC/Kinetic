using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LocalizationSystem
{
    public enum Language
    {
        _First,
        English,
        Brasileiro,
        Deutsch,
        _Last
    }

    static Language CurrentLanguage = Language.English;
    static Dictionary<Language, Dictionary<string, string>> masterDict;
    public static UnityAction OnLanguageUpdate;
    static CSVLoader csvLoader = null;
    static bool IsInit = false;


    public static string GetLocalizedText(string key)
    {
        return GetLocalizedText(key, CurrentLanguage);
    }


    public static string GetLocalizedText(string key, Language lang)
    {
        if (!IsInit)
            Init();

        Debug.Assert(masterDict[lang].Count > 0);
        /*Debug.Log("Size of masterDict[lang]: " + masterDict[lang].Count);
        foreach (string k in masterDict[lang].Keys)
            Debug.Log("In masterDict " + lang.ToString() + ": " + k);*/
        return masterDict.ContainsKey(lang) ? (masterDict[lang].ContainsKey(key) ? masterDict[lang][key] : "") : "";
    }

    public static Dictionary<string, string> GetLanguageDict(Language lang)
    {
        if (!IsInit)
            Init();

        return masterDict[lang];
    }

    public static void UpdateLanguage(Language newLanguage)
    {
        CurrentLanguage = newLanguage;
        OnLanguageUpdate?.Invoke();
    }

    static void Init()
    {
        IsInit = true;
        csvLoader = new CSVLoader("localization");
        UpdateDicts();
    }

    static void UpdateDicts()
    {
        masterDict = new Dictionary<Language, Dictionary<string, string>>();
        for (Language i = Language._First+1; i < Language._Last; i++)
            masterDict.Add(i, csvLoader.GetDictionaryColumn((int)i));
    }

#if UNITY_EDITOR
    public static void Add(string key, int lang, string value)
    {
        if (value.Contains("\""))
            value.Replace('"', '\"');

        if (!IsInit)
            Init();

        // Add brand new line
        if (!csvLoader.LineExists(key))
        {
            string[] values = new string[lang];
            for (int i = 0; i < lang - 1; i++)
                values[i] = "";
            values[lang - 1] = value;

            csvLoader.AddLine(key, values);
        }

        // Insert into existing line
        else
            csvLoader.SetCell(key, lang, value);

        csvLoader.Load();
        UpdateDicts();
    }

    public static void Replace(string key, int lang, string value)
    {
        if (value.Contains("\n"))
            value.Replace("\n", "\\n");
        if (value.Contains("\""))
            value.Replace('"', '\"');

        if (!IsInit)
            Init();

        csvLoader.SetCell(key, lang, value);
        csvLoader.Load();
        UpdateDicts();

    }
#endif
}
