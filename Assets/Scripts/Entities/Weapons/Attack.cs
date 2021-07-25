using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int Damage = 1;

    public void InflictDamage(GameObject target)
    {
        if (target.GetComponent<Damageable>())
            target.GetComponent<Damageable>().InflictDamage(Damage);
    }
}
