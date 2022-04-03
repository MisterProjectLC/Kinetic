using UnityEngine;

public class HoverPhysics : IEnemyPhysics
{
    public float AirDesacceleration = 0f;
    public override RaycastHit RayToGround()
    {
        //Debug.Log(pathAgent.updatePosition);

        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hitInfo, HoverHeight + 1f, GroundLayers.layers, QueryTriggerInteraction.Ignore);
        return hitInfo;
    }

    public override void Stop()
    {
        moveVelocity = Vector3.zero;
    }

    protected override void FeelGravity()
    {
        RaycastHit hit = RayToGround();
        airborne = (hit.collider == null);

        // Air
        if (airborne)
            moveVelocity += Vector3.down * GravityMultiplier * 0.5f * Constants.Gravity * Time.deltaTime;
        // Ground
        else
        {
            // Stop on the ground
            if (moveVelocity.y < 0f)
                moveVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);

            //Slowdown
            moveVelocity = Vector3.MoveTowards(moveVelocity, Vector3.zero, Mathf.Max(1f, moveVelocity.magnitude) * Time.deltaTime);
        }
    }


    protected override void FeelKnockback()
    {
        // Automatic desacceleration (for airborne enemies)
        if (AirDesacceleration > 0 && moveVelocity.magnitude > 0)
        {
            moveVelocity -= moveVelocity.normalized * AirDesacceleration * Time.deltaTime;
            if (moveVelocity.magnitude < 0.5f)
                moveVelocity = Vector3.zero;
        }

        
        // Collision
        Ray ray = new Ray(Model.transform.position, moveVelocity.normalized);
        Physics.Raycast(ray, out RaycastHit hitInfo, CollisionDistance * moveVelocity.magnitude / 12f,
                GroundLayers.layers, QueryTriggerInteraction.Ignore);
        if (hitInfo.collider)
        {
            OnKnockbackCollision?.Invoke(moveVelocity);
            moveVelocity = Vector3.zero;
        }

        // Movement
        transform.position += moveVelocity * Time.deltaTime;
    }

    public override void ReceiveForce(Vector3 force, bool sticky = false)
    {
        if (sticky && !(Vector3.Dot(force, moveVelocity) < 0f || force.magnitude > moveVelocity.magnitude))
            return;

        moveVelocity += force / (2 * Weight);

        // Stop on the ground
        if (moveVelocity.y < 0f && RayToGround().collider)
            moveVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);
    }

    public override void WarpPosition(Vector3 newPosition)
    {
        airborne = true;
        transform.position = newPosition;
    }
}
