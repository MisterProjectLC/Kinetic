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
    Dropdown resolutionDropdown;

    [SerializeField]
    List<OptionData> optionData;

    List<Resolution> resolutions = new List<Resolution>();

    new void Start()
    {
        for (LocalizationSystem.Language l = LocalizationSystem.Language._First + 1; l < LocalizationSystem.Language._Last; l++)
            languageDropdown.options.Add(new Dropdown.OptionData(l.ToString().ToUpper()));

        int currentRes = -1, x = 0, largestRes = 0;
        foreach (Resolution res in Screen.resolutions) {
            if (resolutions.Exists((Resolution ress) => { return ress.width == res.width && ress.height == res.height; }))
                continue;

            if (Screen.currentResolution.width == res.width && Screen.currentResolution.height == res.height)
            {
                currentRes = x;
                //Debug.Log("CurrentRes " + Screen.currentResolution.width + "x" + Screen.currentResolution.height + ", ID = " + currentRes);
            }

            if (x > 0 && res.width > resolutions[largestRes].width)
            {
                largestRes = x;
            }

            resolutionDropdown.options.Add(new Dropdown.OptionData(res.width.ToString() + "x" + res.height.ToString()));
            resolutions.Add(res);
            x++;
        }

        if (currentRes == -1)
        {
            currentRes = largestRes;
        }

        if (Hermes.GetInt(Hermes.Properties.Resolution) == -1)
        {
            Hermes.SetProperty(Hermes.Properties.Resolution, currentRes);
            resolutionDropdown.value = currentRes;
            OnResolutionUpdate(currentRes);
        }

        AddSliderListener(Hermes.Properties.SoundVolume, OnSoundUpdate);
        AddSliderListener(Hermes.Properties.MusicVolume, OnMusicUpdate);
        AddToggleListener(Hermes.Properties.Fullscreen, OnFullscreenToggle);
        AddToggleListener(Hermes.Properties.OutlineEnabled, OnOutlineToggle);
        AddDropdownListener(Hermes.Properties.Language, OnLanguageUpdate);
        AddDropdownListener(Hermes.Properties.Resolution, OnResolutionUpdate);

        foreach (OptionData option in optionData)
        {
            if (option.slider)
            {
                option.slider.onValueChanged.AddListener((v) => Hermes.SetProperty(option.property, option.slider.value));
                option.slider.value = Hermes.IsInit(option.property) ? Hermes.GetFloat(option.property) : option.defaultFloat;
                option.slider.onValueChanged?.Invoke(option.slider.value);
                
            }
            else if (option.toggle)
            {
                option.toggle.onValueChanged.AddListener((v) => Hermes.SetProperty(option.property, option.toggle.isOn));
                option.toggle.isOn = Hermes.IsInit(option.property) ? Hermes.GetBool(option.property) : option.defaultBool;
                option.toggle.onValueChanged?.Invoke(option.toggle.isOn);
            }
            else if (option.dropdown)
            {
                option.dropdown.onValueChanged.AddListener((v) => Hermes.SetProperty(option.property, option.dropdown.value));
                option.dropdown.value = Hermes.IsInit(option.property) ? Hermes.GetInt(option.property) : option.defaultInt;
                option.dropdown.onValueChanged?.Invoke(option.dropdown.value);
            }
        }

        //Debug.Log("Language " + Hermes.GetInt(Hermes.Properties.Language));
        //Debug.Log("Resolution " + Hermes.GetInt(Hermes.Properties.Resolution));


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


    public void OnResolutionUpdate(int value)
    {
        Resolution res = resolutions[value];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
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
