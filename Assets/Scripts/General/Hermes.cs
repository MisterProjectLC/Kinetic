using System.Collections.Generic;
using UnityEngine;

public class Hermes : MonoBehaviour
{
    public static string heroName = "";

    static float soundVolume = 0.5f;
    public static float SoundVolume
    {
        get { return soundVolume; }
        set {
            PlayerPrefs.SetFloat("SoundVolume", value);
            soundVolume = value; 
        }
    }

    static float musicVolume = 0.5f;
    public static float MusicVolume
    {
        get { return musicVolume; }
        set {
            PlayerPrefs.SetFloat("MusicVolume", value);
            musicVolume = value; 
        }
    }

    static float mouseSensibility = 1f;
    public static float MouseSensibility
    {
        get { return mouseSensibility; }
        set {
            PlayerPrefs.SetFloat("MouseSensibility", value);
            mouseSensibility = value; 
        }
    }

    static bool mouseInvert = false;
    public static bool MouseInvert { 
        get { return mouseInvert; }
        set
        {
            PlayerPrefs.SetInt("MouseInvert", value ? 1 : 0);
            mouseInvert = value;
        }
    }

    static float outlineEnabled = 0.5f;
    public static bool OutlineEnabled
    {
        get { return outlineEnabled > 0f; }
        set
        {
            PlayerPrefs.SetFloat("OutlineEnabled", value ? 0.5f : 0f);
            outlineEnabled = value ? 0.5f : 0f;
        }
    }


    // This will just run on a dummy instance whose only job is literally just to load stuff from the PlayerPrefs
    private void Awake()
    {
        soundVolume = PlayerPrefs.GetFloat("SoundVolume");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        mouseSensibility = PlayerPrefs.GetFloat("MouseSensibility");
        mouseInvert = PlayerPrefs.GetInt("MouseInvert") == 1;
        outlineEnabled = PlayerPrefs.GetFloat("OutlineEnabled");
    }


    public static Vector3 SpawnPosition = Vector3.zero;
    public static List<string> SpawnAreas = new List<string>() { "L_Engi_Tutorial" };

    [System.Serializable]
    public struct SavedAbility { public string name; public int loadout; public int slot; public bool inGame;
        public SavedAbility(string name, int loadout, int slot)
        {
            this.name = name; this.loadout = loadout; this.slot = slot; inGame = true;
        }

        public void SetInGame(bool t) { inGame = t; }
    }
    public static List<SavedAbility> SpawnAbilities = new List<SavedAbility>();

}
