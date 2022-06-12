using System.Collections.Generic;
using UnityEngine;

public class Movepad : Pad
{
    [Header("Vectors")]
    [SerializeField]
    Vector3 moveVector;

    [Header("Stats")]
    public float Speed = 5f;
    [SerializeField]
    private bool isJump = true;
    public bool Sticky = false;

    new protected void Start()
    {
        cooldown = isJump ? 0.5f : cooldown;

        if (DespawnPoint && SpawnPoint)
            moveVector = (DespawnPoint.position - SpawnPoint.position).normalized * Speed;

        base.Start();
    }

    protected override void ApplyEffect(GameObject target)
    {
        PhysicsEntity entity = target.GetComponentInParent<PhysicsEntity>();
        Debug.Log("ApplyEffect on " + target.name);

        if (isJump)
            entity.SetMoveVelocity(GetMoveDirection());
        
        else if (Sticky)
            entity.ReceiveForce(GetMoveDirection(), Sticky);

        else
            entity.ReceiveMotion(GetMoveDirection());
    }

    public Vector3 GetMoveDirection()
    {
        return moveVector.magnitude * transform.TransformVector(moveVector).normalized;
    }

    public void SetMoveDirection(Vector3 value)
    {
        moveVector = value;
    }
}
