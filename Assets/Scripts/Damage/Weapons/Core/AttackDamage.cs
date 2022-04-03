using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : Attack
{

    [Header("Attributes")]
    [SerializeField]
    int damage = 1;
    int actualDamage = 1;

    public int Damage
    {
        get { return actualDamage; }
        set { actualDamage = value; }
    }

    protected override void AttackAwake()
    {
        actualDamage = damage;
    }

    public override void AttackTarget(GameObject target, float multiplier = 1f)
    {
        Damageable targetHit = target.GetComponent<Damageable>();


        void OnDamageInvoke(int damage)
        {
            OnAttack?.Invoke(target, multiplier * targetHit.DamageSensitivity, damage);
        }

        // Damage
        if (targetHit)
        {
            targetHit.OnDamage += OnDamageInvoke;
            targetHit.InflictDamage((int)(multiplier * actualDamage), this);
            targetHit.OnDamage -= OnDamageInvoke;
        }
    }
}
