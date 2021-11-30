using System.Collections.Generic;
using UnityEngine;

public class Hermes
{
    public static string heroName = "";

    public static float SoundVolume = 0.75f;
    public static float MusicVolume = 1f;
    public static float mouseSensibility = 1f;
    public static float outlineThickness { get; private set; } = 0.5f;

    public static Vector3 SpawnPosition = Vector3.zero;
    public static List<string> SpawnAreas = new List<string>() { "L_Engi_Tutorial" };

    public static void setOutlineThickness(float value)
    {
        outlineThickness = value;
    }
}
