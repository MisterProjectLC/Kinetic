using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [Header("Attributes")]
    public int Damage = 1;
    public float Knockback = 0f;
    public StatusEffect[] Effects;

    [HideInInspector]
    public UnityAction OnAttack;
    [HideInInspector]
    public UnityAction OnKill;

    private void Start()
    {
        OnKill += Kill;
    }

    void Kill()
    {
        Debug.Log("kill: " + gameObject.name);
    }


    public void AttackTarget(GameObject target)
    {
        AttackTarget(target, 1f);
    }

    public void AttackTarget(GameObject target, float multiplier)
    {
        // Damage
        if (target.GetComponent<Damageable>())
        {
            if (OnAttack != null)
                OnAttack.Invoke();
            target.GetComponent<Damageable>().InflictDamage((int)(multiplier*Damage), this);
        }

        // Effects
        if (target.GetComponentInParent<Enemy>())
            foreach(StatusEffect effect in Effects)
                effect.OnApply(target.GetComponentInParent<Enemy>());

        // Knockback
        Vector3 knockbackForce = (target.transform.position - transform.position).normalized * (int)(multiplier * Knockback);
        if (target.GetComponentInParent<Enemy>())
            target.GetComponentInParent<Enemy>().ReceiveKnockback(knockbackForce);

        else if (target.GetComponent<PlayerCharacterController>())
            target.GetComponent<PlayerCharacterController>().ApplyForce(knockbackForce);
    }
}
