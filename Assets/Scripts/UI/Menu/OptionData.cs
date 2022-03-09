using UnityEngine.UI;

[System.Serializable]
public class OptionData
{
    public enum Components
    {
        _First,
        Slider,
        Toggle,
        Dropdown,
        _Last
    }

    public Components selected = Components.Slider;

    public Slider slider;
    public float defaultFloat;

    public Toggle toggle;
    public bool defaultBool;

    public Dropdown dropdown;
    public int defaultInt;

    public Hermes.Properties property = Hermes.Properties.SoundVolume;
}
