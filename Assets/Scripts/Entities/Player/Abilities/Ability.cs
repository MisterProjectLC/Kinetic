using UnityEngine;
using UnityEngine.Events;

public abstract class Ability : MonoBehaviour
{
    public string DisplayName = "";

    [Tooltip("Cooldown")]
    public float Cooldown = 5f;
    public float Timer { get; protected set; } = 0f;

    public enum Input {
        ButtonUp,
        ButtonDown
    }

    public bool HoldAbility = false;
    public bool ReleaseAbility = false;

    [SerializeField]
    bool EmitSoundEffectOnExecute = false;

    public UnityAction<Input> OnExecute;
    protected UnityAction OnUpdate;

    private void Update()
    {
        Timer = Timer > 0f ? Timer - Time.deltaTime : 0f;
        if (OnUpdate != null)
            OnUpdate.Invoke();
    }


    public void Activate()
    {
        Activate(Input.ButtonDown);
    }

    public void Activate(Input input)
    {
        if (Timer <= 0f)
        {
            Timer = Cooldown;
            Execute(input);
            if (EmitSoundEffectOnExecute)
                GetComponent<AudioSource>().Play();
            if (OnExecute != null)
                OnExecute.Invoke(input);
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
}
