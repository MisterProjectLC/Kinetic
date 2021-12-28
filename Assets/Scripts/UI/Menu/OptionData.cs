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
    public Toggle toggle;
    public Dropdown dropdown;
    public Hermes.Properties property = Hermes.Properties.SoundVolume;
}
