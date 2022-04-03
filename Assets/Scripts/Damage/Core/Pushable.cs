using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour, IPushable
{
    [Range(0, 3f)]
    public float PushSensitivity = 1f;

    [SerializeField]
    PhysicsEntity entity;
    [SerializeField]
    Health health;

    void Awake()
    {
        if (!entity)
            entity = GetComponent<PhysicsEntity>();
        if (!health)
            health = GetComponent<Health>();
    }

    public void ReceiveForce(Vector3 force, Attack attack, bool isSticky = false)
    {
        if (entity)
            entity.ReceiveForce(PushSensitivity*force, isSticky);

        if (health)
            health.InflictDamage(0, attack);
    }
}
