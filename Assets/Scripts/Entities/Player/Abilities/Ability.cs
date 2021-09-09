using UnityEngine;
using UnityEngine.Events;

public abstract class Ability : MonoBehaviour
{
    public string DisplayName = "";

    [Tooltip("Cooldown")]
    public float Cooldown = 5f;
    public float Timer { get; protected set; } = 0f;

    public bool HoldAbility = false;

    public UnityAction OnExecute;
    protected UnityAction OnUpdate;

    private void Update()
    {
        Timer = Timer > 0f ? Timer - Time.deltaTime : 0f;
        if (OnUpdate != null)
            OnUpdate.Invoke();
    }

    public void Activate()
    {
        if (Timer <= 0f)
        {
            Timer = Cooldown;
            Execute();
            if (OnExecute != null)
                OnExecute.Invoke();
        }
    }

    public abstract void Execute();

    public void SetOffCooldown()
    {
        Timer = Cooldown;
    }

    public void ResetCooldown()
    {
        Timer = 0;
    }
}
