using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [HideInInspector]
    public static MusicManager MM;

    GameObject player;
    [SerializeField]
    GameObjectReference playerReference;

    [SerializeField]
    Transform objective;

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
        player = playerReference.Reference;
        player.GetComponent<StyleMeter>().SubscribeToCritical(OnCritical);
    }


    private void Update()
    {
        if (BaseMusic.time >= 120 && targetVolume == 0f)
        {
            BaseMusic.time = 7;
            StyleMusic.time = 7;
        }

        StyleMusic.volume = Mathf.Lerp(StyleMusic.volume, targetVolume, Time.deltaTime);

        if (player && objective)
        {
            Vector3 direction = player.transform.InverseTransformDirection((objective.position - player.transform.position)).normalized;
            BaseMusic.panStereo = direction.x;
        }
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
