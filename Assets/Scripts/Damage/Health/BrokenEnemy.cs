using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenEnemy : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Vector3 velocity;

    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
        Debug.Log("velocity: " + velocity);
    }

    private void FixedUpdate()
    {
        //foreach (Rigidbody rigidbody in rigidbodies)
        //Debug.Log("velocity: " + rigidbody.velocity);
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.AddForce(velocity / 2, ForceMode.Force);
            velocity = Vector3.Lerp(velocity, Vector3.zero, 0.01f);
        }
        
    }
}
