using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectApplier : Attack
{
    [System.Serializable]
    public struct StatusEffectApplied
    {
        public StatusEffect effect;
        public float duration;
    }

    public float Knockback = 0f;
    public List<StatusEffectApplied> Effects;


    public override void AttackTarget(GameObject target, float multiplier = -1)
    {
        Damageable targetHit = target.GetComponent<Damageable>();
        if (targetHit)
            targetHit.InflictDamage(0, this);

        // Effects
        StatusEffectReceiver receiver = target.GetComponentInParent<StatusEffectReceiver>();
        if (receiver != null)
            foreach (StatusEffectApplied effect in Effects)
            {
                effect.effect.ApplyEffect(receiver);
                receiver.ReceiveStatusEffect(effect.effect, effect.duration);
            }
    }
}
