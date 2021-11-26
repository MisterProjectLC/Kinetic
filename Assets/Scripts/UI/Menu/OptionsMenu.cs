using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OptionsMenu : MonoBehaviour
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
    Slider outlineSlider;

    [SerializeField]
    LineRenderer lastTry;

    [SerializeField]
    SliderData[] sliders;

    List<ScriptableRendererFeature> features;

    private void Start()
    {
        soundSlider.onValueChanged.AddListener(OnSoundUpdate);
        musicSlider.onValueChanged.AddListener(OnMusicUpdate);
        mouseSlider.onValueChanged.AddListener(OnMouseUpdate);
        outlineSlider.onValueChanged.AddListener(OnOutlineUpdate);

        soundSlider.value = Hermes.SoundVolume;
        musicSlider.value = Hermes.MusicVolume;
        mouseSlider.value = Hermes.mouseSensibility;
        outlineSlider.value = Hermes.outlineThickness;
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

    void OnOutlineUpdate(float value)
    {
        //Getting
        var pipeline = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
        FieldInfo propertyInfo = pipeline.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
        ScriptableRendererData[]  rendererData = (ScriptableRendererData[])propertyInfo?.GetValue(pipeline);
        OutlineFeature outlineFeature = (OutlineFeature)rendererData[0].rendererFeatures[4];
        Material m = outlineFeature.settings.outlineMaterial;

        //Setting
        m.SetFloat("Vector1_e96a4ec09abd4fab8bb36e836b4e1a9d", value);
        outlineFeature.settings.outlineMaterial = m;
        rendererData[0].rendererFeatures[4] = outlineFeature;
        rendererData[0].SetDirty();
        propertyInfo.SetValue(pipeline, rendererData);
        GraphicsSettings.renderPipelineAsset = pipeline;
        Hermes.setOutlineThickness(value);


        pipeline = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
        propertyInfo = pipeline.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
        rendererData = (ScriptableRendererData[])propertyInfo?.GetValue(pipeline);
        outlineFeature = (OutlineFeature)rendererData[0].rendererFeatures[4];
        m = outlineFeature.settings.outlineMaterial;
        Debug.Log(m.GetFloat("Vector1_e96a4ec09abd4fab8bb36e836b4e1a9d"));
    }

    void InitOutline()
    {
        outlineSlider.value = Hermes.outlineThickness;
    }
}
