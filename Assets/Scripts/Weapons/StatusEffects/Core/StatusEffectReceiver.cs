using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StatusEffectReceiver
{
    public void ReceiveStatusEffect(StatusEffect statusEffect, float duration);
    public bool HasStatusEffect(StatusEffect statusEffect);
    public bool HasAnyOfTheseStatusEffects(List<StatusEffect> statusEffects);
}
