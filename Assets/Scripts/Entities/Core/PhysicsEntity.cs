using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PhysicsEntity
{
    public GameObject GetGameObject();
    public Vector3 GetMoveVelocity();
    public abstract void SetMoveVelocity(Vector3 velocity);
    public abstract void WarpPosition(Vector3 newPosition);
    public abstract void ReceiveForce(Vector3 force, bool isSticky = false);
    public abstract void ReceiveMotion(Vector3 motion);
}
