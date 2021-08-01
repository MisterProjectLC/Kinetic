using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Radius = 5f;
    //public float KnockbackForce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Attack attack = GetComponent<Attack>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach(Collider collider in colliders)
            attack.AttackTarget(collider.gameObject);
    }
}
