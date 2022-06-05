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

    [SerializeField]
    private float cooldownOverride = -1f;

    new protected void Start()
    {
        cooldown = isJump ? 0.5f : cooldown;

        if (DespawnPoint && SpawnPoint)
        {
            moveVector = transform.InverseTransformVector(DespawnPoint.localPosition - SpawnPoint.localPosition).normalized * Speed;
        }

        base.Start();
    }

    protected override void ApplyEffect(GameObject target)
    {
        Vector3 currentMoveVector = GetMoveDirection();

        PhysicsEntity entity = target.GetComponentInParent<PhysicsEntity>();
        if (isJump)
            entity.SetMoveVelocity(currentMoveVector);
        
        else if (Sticky)
            entity.ReceiveForce(currentMoveVector, Sticky);

        else
            entity.ReceiveMotion(currentMoveVector);
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
