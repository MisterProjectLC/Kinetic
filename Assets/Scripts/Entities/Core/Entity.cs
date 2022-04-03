using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : PhysicsEntity, StatusEffectReceiver
{
    List<StatusEffect> statusEffects;
    Dictionary<StatusEffect, float> statusEffectDurations;

    protected void Awake()
    {
        statusEffects = new List<StatusEffect>();
        statusEffectDurations = new Dictionary<StatusEffect, float>();
    }

    private void Update()
    {
        MyUpdate();

        foreach (StatusEffect statusEffect in statusEffects)
            if (statusEffectDurations[statusEffect] > 0f)
                statusEffectDurations[statusEffect] -= Time.deltaTime;
    }

    protected abstract void MyUpdate();

    public void ReceiveStatusEffect(StatusEffect statusEffect, float duration)
    {
        if (!statusEffect)
        {
            Debug.LogError("Something went wrong - StatusEffect is null!");
            return;
        }

        if (statusEffectDurations.ContainsKey(statusEffect))
            statusEffectDurations[statusEffect] = duration;

        else
        {
            statusEffectDurations.Add(statusEffect, duration);
            statusEffects.Add(statusEffect);
        }
    }

    public bool HasStatusEffect(StatusEffect statusEffect)
    {
        return statusEffectDurations.ContainsKey(statusEffect) && statusEffectDurations[statusEffect] > 0;
    }

    public bool HasAnyOfTheseStatusEffects(List<StatusEffect> statusEffects)
    {
        foreach (StatusEffect statusEffect in statusEffects)
            if (HasStatusEffect(statusEffect))
                return true;
        return false;
    }


    public abstract void FallFatal(float VerticalLimit, Transform FallRespawnPoint);

}
