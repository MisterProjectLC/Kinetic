using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ability : MonoBehaviour
{
    public LocalizedString LocalizedName;

    [Tooltip("Cooldown")]
    public float Cooldown = 5f;
    public float Timer { get; protected set; } = 0f;
    [HideInInspector]
    public bool Assigned = false;

    public enum Input {
        ButtonUp,
        ButtonDown
    }

    public bool HoldAbility = false;
    public bool ReleaseAbility = false;
    bool waiting = true;

    [SerializeField]
    AudioClip[] SoundEffects;
    AudioSource audioSource;

    public UnityAction<Input> OnExecute;
    public UnityAction<Ability> OnExecuteAbility;
    protected UnityAction OnUpdate;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (OnExecute == null)
            OnExecute += (Input input) => OnExecuteAbility?.Invoke(this);

    }

    private void OnEnable()
    {
        if (OnExecute == null)
            OnExecute += (Input input) => OnExecuteAbility?.Invoke(this);

        StartCoroutine(StopWaiting());
    }

    private void Update()
    {
        Timer = Timer > 0f ? Timer - Time.deltaTime : 0f;
        OnUpdate?.Invoke();
    }

    IEnumerator StopWaiting()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        waiting = false;
    }


    public void Activate()
    {
        Activate(Input.ButtonDown);
    }

    public void Activate(Input input)
    {
        if (Timer <= 0f && !waiting)
        {
            Timer = Cooldown;
            Execute(input);
            if (SoundEffects.Length > 0)
                PlaySound(SoundEffects[Random.Range(0, SoundEffects.Length)]);

            OnExecute?.Invoke(input);
        }
    }

    public abstract void Execute(Input input);


    public void SetOffCooldown()
    {
        Timer = Cooldown;
    }

    public void ResetCooldown()
    {
        Timer = 0;
    }

    protected void PlaySound(AudioClip sound)
    {
        if (!audioSource)
            return;
        audioSource.clip = sound;
        audioSource.Play();
    } 
}
