using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceExtension : MonoBehaviour
{
    [SerializeField]
    bool IgnoreListenerPause = false;

    private void Awake()
    {
        Debug.Assert(GetComponent<AudioSource>() != null);
        AudioSource source = GetComponent<AudioSource>();
        source.ignoreListenerPause = IgnoreListenerPause;
    }
}