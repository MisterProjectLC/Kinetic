using UnityEngine;
using UnityEngine.Events;

public class PlayerFallHandler : MonoBehaviour, IFallHandler
{
    public float TimeUnderLimit { get; private set; } = 0f;
    [HideInInspector]
    public float VerticalLimit = -150f;
    [HideInInspector]
    public Vector3 FallRespawnPoint;

    public UnityAction OnFall;
    public UnityAction OnChange;

    // Update is called once per frame
    void Update()
    {
        float previous = TimeUnderLimit;

        if (transform.position.y < VerticalLimit)
        {
            TimeUnderLimit += Time.deltaTime;
            if (TimeUnderLimit > 1f)
            {
                OnFall?.Invoke();
                GetComponentInParent<Health>().InflictDamage(2);
                GetComponentInParent<CharacterController>().transform.position = FallRespawnPoint;
                TimeUnderLimit = 0f;
                Debug.Log("PlayerFallHandler");
            }
        }
        else
            TimeUnderLimit = 0f;

        if (previous != TimeUnderLimit)
            OnChange?.Invoke();
    }

    public void FallFatal(float VerticalLimit, Transform FallRespawnPoint)
    {
        this.VerticalLimit = VerticalLimit;
        this.FallRespawnPoint = FallRespawnPoint.position;
        Debug.Log("FALLFATAL");
    }
}
