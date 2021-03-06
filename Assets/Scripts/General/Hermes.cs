using System.Collections.Generic;
using UnityEngine;

public class Hermes : MonoBehaviour
{
    static bool init = false;

    enum NullableBool
    {
        False = 0,
        True,
        Null
    }

    public enum PlayerClass
    {
        Ninja,
        Vanguard,
        Null
    }

    public enum Properties
    {
        _First,
        SoundVolume,
        MusicVolume,
        FOV,
        MouseSensibility,
        Fullscreen,
        MouseInvert,
        OutlineEnabled,
        Language,
        Resolution,
        _Last
    }

    public static PlayerClass CurrentClass = PlayerClass.Null;

    static Dictionary<Properties, object> properties = new Dictionary<Properties, object>();

    public static void SetProperty(Properties key, object value)
    {
        if (value is float)
            PlayerPrefs.SetFloat(key.ToString(), (float)value);
        else if (value is int)
            PlayerPrefs.SetInt(key.ToString(), (int)value);
        else if (value is bool)
            PlayerPrefs.SetInt(key.ToString(), (bool)value ? 1 : 0);

        if (value is bool)
            value = (bool)value ? NullableBool.True : NullableBool.False;

        if (properties.ContainsKey(key))
            properties[key] = value;
        else
            properties.Add(key, value);
    }

    public static float GetFloat(Properties key)
    {
        Debug.Assert(properties.ContainsKey(key) && properties[key] is float);
        return (float)properties[key];
    }
    public static int GetInt(Properties key)
    {
        Debug.Assert(properties.ContainsKey(key) && properties[key] is int);
        return (int)properties[key];
    }

    public static bool GetBool(Properties key)
    {
        Debug.Assert(properties.ContainsKey(key) && properties[key] is NullableBool);
        return (NullableBool)properties[key] == NullableBool.True;
    }

    public static bool IsInit(Properties key)
    {
        Debug.Assert(properties.ContainsKey(key));
        if (properties[key] is NullableBool)
            return (NullableBool)properties[key] != NullableBool.Null;
        else if (properties[key] is float)
            return (float)properties[key] != -1f;
        else
            return (int)properties[key] != -1;
    }


    static void GenerateFloat(Properties key)
    {
        SetProperty(key, PlayerPrefs.GetFloat(key.ToString(), -1));
    }

    static void GenerateInt(Properties key)
    {
        SetProperty(key, PlayerPrefs.GetInt(key.ToString(), -1));
    }

    static void GenerateBool(Properties key)
    {
        int value = PlayerPrefs.GetInt(key.ToString(), -1);
        if (value != -1)
            SetProperty(key, (NullableBool)value);
        else
            SetProperty(key, NullableBool.Null);
    }


    // This will just run on a dummy instance whose only job is literally just to load stuff from the PlayerPrefs
    private void Awake()
    {
        if (init)
            return;

        init = true;
        GenerateFloat(Properties.SoundVolume);
        GenerateFloat(Properties.MusicVolume);
        GenerateFloat(Properties.FOV);
        GenerateFloat(Properties.MouseSensibility);
        GenerateBool(Properties.Fullscreen);
        GenerateBool(Properties.MouseInvert);
        GenerateBool(Properties.OutlineEnabled);
        GenerateInt(Properties.Language);
        GenerateInt(Properties.Resolution);
    }


    public static Vector3 SpawnPosition = Vector3.zero;
    public static List<string> SpawnAreas = new List<string>() { "L_Engi_Tutorial" };

    public static bool newGame = false;
}
