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
    [System.Serializable]
    public struct SliderData
    {
        public string name;
        public Slider slider;
        public UnityEvent<float> listener;
        public UnityEvent init;
    }

    [SerializeField]
    AudioMixer Mixer;

    [SerializeField]
    Slider soundSlider;
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider mouseSlider;
    [SerializeField]
    Toggle outlineToggle;
    [SerializeField]
    Toggle mouseToggle;

    List<ScriptableRendererFeature> features;

    new void Start()
    {
        soundSlider.onValueChanged.AddListener(OnSoundUpdate);
        soundSlider.value = Hermes.SoundVolume;

        musicSlider.onValueChanged.AddListener(OnMusicUpdate);
        musicSlider.value = Hermes.MusicVolume;

        mouseSlider.onValueChanged.AddListener(OnMouseUpdate);
        mouseSlider.value = Hermes.MouseSensibility;

        outlineToggle.onValueChanged.AddListener(OnOutlineToggle);
        outlineToggle.isOn = Hermes.OutlineEnabled;

        mouseToggle.onValueChanged.AddListener(OnMouseInvertToggle);
        mouseToggle.isOn = Hermes.MouseInvert;

        base.Start();
    }


    void OnSoundUpdate(float value)
    {
        Hermes.SoundVolume = soundSlider.value;
        Mixer.SetFloat("SoundVolume", Mathf.Log10(value) * 20);
    }

    void OnMusicUpdate(float value)
    {
        Hermes.MusicVolume = musicSlider.value;
        Mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    void OnMouseUpdate(float value)
    {
        Hermes.MouseSensibility = value;
    }

    void OnOutlineToggle(bool val)
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
        Hermes.OutlineEnabled = val;
    }

    void OnMouseInvertToggle(bool value)
    {
        Hermes.MouseInvert = value;
    }
}
