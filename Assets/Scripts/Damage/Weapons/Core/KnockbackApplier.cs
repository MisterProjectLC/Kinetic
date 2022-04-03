using UnityEngine;

public class KnockbackApplier : Attack
{
    public float Knockback = 0f;

    public override void AttackTarget(GameObject target, float multiplier = 1)
    {
        Damageable targetHit = target.GetComponent<Damageable>();
        if (targetHit)
            targetHit.InflictDamage(0, this);

        // Knockback
        Vector3 knockbackForce = (target.transform.position - transform.position).normalized * (int)(multiplier * Knockback);
        IPushable pushable = target.GetComponentInParent<IPushable>();
        if (pushable != null)
            pushable.ReceiveForce(knockbackForce, this);
    }
}
