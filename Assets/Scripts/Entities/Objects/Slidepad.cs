using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slidepad : Pad
{
    [Header("Vectors")]
    public Vector3 moveVector;

    [Header("Stats")]
    public float Speed = 5f;

    new protected void Start()
    {
        if (DespawnPoint && SpawnPoint)
            moveVector = (DespawnPoint.position - SpawnPoint.position).normalized * Speed;

        base.Start();
    }

    protected override void ApplyEffect(GameObject target)
    {
        PhysicsEntity entity = target.GetComponentInParent<PhysicsEntity>();
        entity.ReceiveMotion(moveVector);
    }
}
