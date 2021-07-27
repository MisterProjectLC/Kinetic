using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    public int Damage = 1;
    [HideInInspector]
    public UnityAction OnAttack;
    [HideInInspector]
    public UnityAction OnKill;

    public void InflictDamage(GameObject target)
    {
        if (target.GetComponent<Damageable>())
        {
            if (OnAttack != null)
                OnAttack.Invoke();
            target.GetComponent<Damageable>().InflictDamage(Damage, this);
        }
    }
}
