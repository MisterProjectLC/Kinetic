using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectApplier : MonoBehaviour
{
    public float Knockback = 0f;
    public StatusEffect[] Effects;

    private void Start()
    {
        if (!GetComponent<Poolable>())
            GetComponent<Attack>().OnAttack += AffectTarget;
    }

    private void OnEnable()
    {
        if (GetComponent<Poolable>())
            GetComponent<Attack>().OnAttack += AffectTarget;
    }


    public void AffectTarget(GameObject target, float multiplier, int _damage)
    {
        // Effects
        if (target.GetComponentInParent<Enemy>())
            foreach (StatusEffect effect in Effects)
                effect.OnApply(target.GetComponentInParent<Enemy>());

        // Knockback
        Vector3 knockbackForce = (target.transform.position - transform.position).normalized * (int)(multiplier * Knockback);
        if (target.GetComponentInParent<Enemy>())
        {
            target.GetComponentInParent<Enemy>().ReceiveKnockback(knockbackForce);
        }

        else if (target.GetComponent<PlayerCharacterController>())
            target.GetComponent<PlayerCharacterController>().ApplyForce(knockbackForce);

        else if (target.GetComponent<Rigidbody>())
            target.GetComponent<Rigidbody>().AddForce(knockbackForce, ForceMode.Impulse);
    }
}
