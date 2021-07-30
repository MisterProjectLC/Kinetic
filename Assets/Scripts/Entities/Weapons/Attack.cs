using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [Header("Attributes")]
    public int Damage = 1;
    public StatusEffect[] Effects;

    [HideInInspector]
    public UnityAction OnAttack;
    [HideInInspector]
    public UnityAction OnKill;

    public void AttackTarget(GameObject target)
    {
        if (target.GetComponent<Damageable>())
        {
            if (OnAttack != null)
                OnAttack.Invoke();
            target.GetComponent<Damageable>().InflictDamage(Damage, this);
        }

        if (target.GetComponentInParent<Enemy>())
        {
            foreach(StatusEffect effect in Effects)
            {
                effect.OnApply(target.GetComponentInParent<Enemy>());
            }
        }
    }
}
