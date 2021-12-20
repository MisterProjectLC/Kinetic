using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [HideInInspector]
    public static MusicManager MM;

    [SerializeField]
    AudioSource BaseMusic;
    [SerializeField]
    AudioSource StyleMusic;
    float targetVolume = 1f;

    private void Awake()
    {
        MM = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ActorsManager.AM.GetPlayer().GetComponent<StyleMeter>().OnCritical += OnCritical;
    }


    private void Update()
    {
        if (BaseMusic.time >= 120 && targetVolume == 0f)
        {
            BaseMusic.time = 7;
            StyleMusic.time = 7;
        }

        StyleMusic.volume = Mathf.Lerp(StyleMusic.volume, targetVolume, Time.deltaTime);
    }

    void OnCritical(bool critical)
    {
        targetVolume = critical ? 1f : 0f;
    }

    public void ChangeBaseSong(AudioClip audioClip)
    {
        BaseMusic.Stop();
        StyleMusic.Stop();
        BaseMusic.clip = audioClip;
        BaseMusic.Play();
        StyleMusic.Play();
    }
}
