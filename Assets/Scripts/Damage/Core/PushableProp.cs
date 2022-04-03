using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableProp : MonoBehaviour, IPushable
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    public void ReceiveForce(Vector3 force, Attack _attack, bool _isSticky = false)
    {
        if (rb)
            rb.AddForce(force, ForceMode.Impulse);
    }
}
