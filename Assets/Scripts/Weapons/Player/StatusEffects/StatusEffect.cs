using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    struct AffectedEnemy
    {
        public float Duration;
        public Enemy Target;
    }

    [SerializeField]
    protected float TickDuration = 8f;

    [SerializeField]
    protected float TickTime = 0.1f;

    float Clock = 0f;
    static List<AffectedEnemy> AffectedTargets;

    private void Start()
    {
        if (AffectedTargets == null)
            AffectedTargets = new List<AffectedEnemy>(5);
    }

    private void Update()
    {
        Clock += Time.deltaTime;
        if (Clock > TickTime)
        {
            Clock = 0f;
            foreach (AffectedEnemy target in AffectedTargets)
                OnTick(target);
            AffectedTargets.RemoveAll(x => x.Duration <= 0f);
        }
    }

    public void OnApply(Enemy target)
    {
        AffectedTargets.RemoveAll(x => x.Target == target);
        AffectedTargets.Add(new AffectedEnemy() { Duration = TickDuration, Target = target });

        ApplyEffect(target);
    }

    protected abstract void ApplyEffect(Enemy target);

    void OnTick(AffectedEnemy target)
    {
        TickEffect(target.Target);
        target.Duration -= Time.deltaTime;
        if (target.Duration <= 0f)
            EndEffect(target.Target);
    }

    protected abstract void TickEffect(Enemy target);

    protected abstract void EndEffect(Enemy target);
}
