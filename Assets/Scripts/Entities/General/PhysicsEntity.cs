using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicsEntity : MonoBehaviour
{
    public float HoverHeight = 1f;
    public float GravityMultiplier = 1f;
    protected float lastGravityMultiplier = 1f;
    public float AirDesacceleration = 0f;
    public float Weight = 1f;

    private void Update()
    {
        FeelGravity();
        FeelKnockback();

        MyUpdate();
    }

    protected abstract void MyUpdate();
    protected abstract void FeelGravity();
    protected abstract void FeelKnockback();

    public abstract Vector3 GetMoveVelocity();
    public abstract void WarpPosition(Vector3 newPosition);
    public abstract void ReceiveKnockback(Vector3 knockback);
}
