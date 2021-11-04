using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [Header("Attributes")]
    public int Damage = 1;

    [HideInInspector]
    public UnityAction<GameObject, float, int> OnAttack;
    [HideInInspector]
    public UnityAction OnKill;

    private void Start()
    {
        OnKill += Kill;
    }

    void Kill()
    {
        //Debug.Log("kill: " + gameObject.name);
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
            OnAttack?.Invoke(target, multiplier, (int)(multiplier * Damage));
            target.GetComponent<Damageable>().InflictDamage((int)(multiplier*Damage), this);
        }
    }
}
