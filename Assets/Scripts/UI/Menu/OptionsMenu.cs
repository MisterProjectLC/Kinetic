using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    AudioMixer Mixer;

    [SerializeField]
    Slider soundSlider;

    [SerializeField]
    Slider musicSlider;

    [SerializeField]
    Slider mouseSlider;

    private void Start()
    {
        soundSlider.onValueChanged.AddListener(OnSoundUpdate);
        musicSlider.onValueChanged.AddListener(OnMusicUpdate);
        mouseSlider.onValueChanged.AddListener(OnMouseUpdate);

        soundSlider.value = Hermes.SoundVolume;
        musicSlider.value = Hermes.MusicVolume;
        mouseSlider.value = Hermes.mouseSensibility;
    }

    void OnSoundUpdate(float value)
    {
        Hermes.SoundVolume = soundSlider.value;
        Mixer.SetFloat("SoundVolume", Mathf.Log10(value) * 20);
    }

    void OnMusicUpdate(float value)
    {
        Hermes.MusicVolume = soundSlider.value;
        Mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    void OnMouseUpdate(float value)
    {
        Hermes.mouseSensibility = value;
    }
}
