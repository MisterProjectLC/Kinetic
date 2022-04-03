using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public abstract void ApplyEffect(StatusEffectReceiver target);
}
