using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioSource StyleMusic;
    float targetVolume = 1f;

    StyleMeter styleMeter;

    // Start is called before the first frame update
    void Start()
    {
        styleMeter = ActorsManager.AM.GetPlayer().GetComponent<StyleMeter>();
        ActorsManager.AM.GetPlayer().GetComponent<StyleMeter>().OnCritical += OnCritical;
    }

    private void Update()
    {
        StyleMusic.volume = Mathf.Lerp(StyleMusic.volume, targetVolume, Time.deltaTime);
    }

    void OnCritical(bool critical)
    {
        targetVolume = critical ? 1f : 0f;
    }
}
