using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class IEnemyPhysics : MonoBehaviour, SubcomponentUpdate
{
    protected Vector3 moveVelocity = Vector3.zero;
    public Vector3 MoveVelocity { get { return moveVelocity; } }

    [Header("References")]
    public LayersConfig GroundLayers;

    [Header("Attributes")]
    [SerializeField]
    protected float CollisionDistance = 1f;
    public float HoverHeight = 1f;
    public float GravityMultiplier = 1f;
    protected float lastGravityMultiplier = 1f;
    public float Weight = 1f;

    protected GameObject Model;
    protected bool airborne = true;

    protected UnityAction<Vector3> OnKnockbackCollision;
    public void SubscribeToUpdate(UnityAction<Vector3> subscriber) { OnKnockbackCollision += subscriber; }


    public void Setup(GameObject model)
    {
        Model = model;
    }


    public void OnUpdate()
    {
        FeelGravity();
        FeelKnockback();
    }

    public float GetCollisionDistance()
    {
        return CollisionDistance;
    }

    abstract public RaycastHit RayToGround();
    abstract protected void FeelGravity();
    abstract protected void FeelKnockback();

    public void DeactivateGravity()
    {
        if (GravityMultiplier < 1f)
        {
            lastGravityMultiplier = GravityMultiplier;
            GravityMultiplier = 0f;
        }
    }

    public void ReactivateGravity()
    {
        GravityMultiplier = lastGravityMultiplier;
    }

    public abstract void Stop();

    public Vector3 GetMoveVelocity()
    {
        return moveVelocity;
    }

    public void SetMoveVelocity(Vector3 velocity)
    {
        moveVelocity = velocity;
    }

    public abstract void WarpPosition(Vector3 newPosition);

    public abstract void ReceiveForce(Vector3 force, bool isSticky = false);

    public void ReceiveMotion(Vector3 motion)
    {
        ReceiveForce(motion, true);
    }
}
