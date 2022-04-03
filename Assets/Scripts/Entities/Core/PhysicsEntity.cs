using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsEntity : MonoBehaviour
{
    protected Vector3 moveVelocity = Vector3.zero;
    public Vector3 MoveVelocity { get { return moveVelocity; } }

    public float HoverHeight = 1f;
    public float GravityMultiplier = 1f;
    protected float lastGravityMultiplier = 1f;
    public float AirDesacceleration = 0f;
    public float Weight = 1f;

    public Vector3 GetMoveVelocity()
    {
        return moveVelocity;
    }

    public abstract void SetMoveVelocity(Vector3 velocity);

    public abstract void WarpPosition(Vector3 newPosition);
    public abstract void ReceiveForce(Vector3 force, bool isSticky = false);
    public abstract void ReceiveMotion(Vector3 motion);
}
