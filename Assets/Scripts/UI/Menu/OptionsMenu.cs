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

    private void Start()
    {
        soundSlider.onValueChanged.AddListener(OnSoundUpdate);
        musicSlider.onValueChanged.AddListener(OnMusicUpdate);
    }

    void OnSoundUpdate(float value)
    {
        Mixer.SetFloat("SoundVolume", Mathf.Log10(value) * 20);
    }

    void OnMusicUpdate(float value)
    {
        Mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
}
