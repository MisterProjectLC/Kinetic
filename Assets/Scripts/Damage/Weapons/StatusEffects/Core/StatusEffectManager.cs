using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : StatusEffectReceiver, SubcomponentUpdate
{
    List<StatusEffect> statusEffects;
    Dictionary<StatusEffect, float> statusEffectDurations;

    public StatusEffectManager()
    {
        statusEffects = new List<StatusEffect>();
        statusEffectDurations = new Dictionary<StatusEffect, float>();
    }

    public void OnUpdate()
    {
        foreach (StatusEffect statusEffect in statusEffects)
            if (statusEffectDurations[statusEffect] > 0f)
                statusEffectDurations[statusEffect] -= Time.deltaTime;
    }


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
}
