using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OptionsMenu : Menu
{
    [SerializeField]
    AudioMixer Mixer;

    [SerializeField]
    Dropdown languageDropdown;

    [SerializeField]
    List<OptionData> optionData;

    new void Start()
    {
        for (LocalizationSystem.Language l = LocalizationSystem.Language._First + 1; l < LocalizationSystem.Language._Last; l++)
            languageDropdown.options.Add(new Dropdown.OptionData(l.ToString().ToUpper()));

        AddSliderListener(Hermes.Properties.SoundVolume, OnSoundUpdate);
        AddSliderListener(Hermes.Properties.MusicVolume, OnMusicUpdate);
        AddToggleListener(Hermes.Properties.Fullscreen, OnFullscreenToggle);
        AddToggleListener(Hermes.Properties.OutlineEnabled, OnOutlineToggle);
        AddDropdownListener(Hermes.Properties.Language, OnLanguageUpdate);

        foreach (OptionData option in optionData)
        {
            if (option.slider)
            {
                option.slider.onValueChanged.AddListener((v) => Hermes.SetProperty(option.property, option.slider.value));
                option.slider.value = Hermes.GetFloat(option.property);
                option.slider.onValueChanged?.Invoke(Hermes.GetFloat(option.property));
            }
            else if (option.toggle)
            {
                option.toggle.onValueChanged.AddListener((v) => Hermes.SetProperty(option.property, option.toggle.isOn));
                option.toggle.isOn = Hermes.GetBool(option.property);
                option.toggle.onValueChanged?.Invoke(Hermes.GetBool(option.property));
            }
            else if (option.dropdown)
            {
                option.dropdown.onValueChanged.AddListener((v) => Hermes.SetProperty(option.property, option.dropdown.value));
                option.dropdown.value = Hermes.GetInt(option.property);
                option.dropdown.onValueChanged?.Invoke(Hermes.GetInt(option.property));
            }
        }

        base.Start();
    }


    public void OnSoundUpdate(float value)
    {
        Mixer.SetFloat("SoundVolume", Mathf.Log10(value) * 20);
    }

    public void OnMusicUpdate(float value)
    {
        Mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void OnFullscreenToggle(bool value)
    {
        Screen.fullScreen = value;
    }

    public void OnOutlineToggle(bool val)
    {
        float value = val ? 0.5f : 0f;

        //Getting
        var pipeline = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
        FieldInfo propertyInfo = pipeline.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
        ScriptableRendererData[]  rendererData = (ScriptableRendererData[])propertyInfo?.GetValue(pipeline);
        OutlineFeature outlineFeature = (OutlineFeature)rendererData[0].rendererFeatures[4];
        Material m = outlineFeature.settings.outlineMaterial;

        //Setting
        m.SetFloat("_OutlineThickness", value);
        outlineFeature.settings.outlineMaterial = m;
        rendererData[0].rendererFeatures[4] = outlineFeature;
        rendererData[0].SetDirty();
        propertyInfo.SetValue(pipeline, rendererData);
        GraphicsSettings.renderPipelineAsset = pipeline;
    }

    public void OnLanguageUpdate(int value)
    {
        LocalizationSystem.UpdateLanguage(LocalizationSystem.Language._First + 1 + value);
    }


    void AddSliderListener(Hermes.Properties property, UnityAction<float> call)
    {
        optionData.Find(v => v.property == property).slider.onValueChanged.AddListener(call);
    }

    void AddToggleListener(Hermes.Properties property, UnityAction<bool> call)
    {
        optionData.Find(v => v.property == property).toggle.onValueChanged.AddListener(call);
    }

    void AddDropdownListener(Hermes.Properties property, UnityAction<int> call)
    {
        optionData.Find(v => v.property == property).dropdown.onValueChanged.AddListener(call);
    }
}
