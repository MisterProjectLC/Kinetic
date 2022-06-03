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

    RaycastHit[] hits = new RaycastHit[1];
    float lastRayDistance = 0f;

    public override void Stop()
    {
        moveVelocity = Vector3.zero;
    }

    protected override void FeelGravity()
    {
        if (lastRayDistance < 5f)
        {
            RaycastHit hit = RayToGround();
            airborne = (hit.collider == null);
            lastRayDistance = hit.distance;
        }
        else
            airborne = true;

        // Air
        if (airborne)
        {
            Vector3 adder = Vector3.down;
            float factor = (GravityMultiplier * 0.5f * Constants.Gravity * Time.deltaTime);
            adder.x *= factor;
            adder.y *= factor;
            adder.z *= factor;
            moveVelocity.x += adder.x;
            moveVelocity.y += adder.y;
            moveVelocity.z += adder.z;
        }
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
            Vector3 subtractor = moveVelocity.normalized;
            float factor = AirDesacceleration * Time.deltaTime;
            subtractor.x *= factor;
            subtractor.y *= factor;
            subtractor.z *= factor;
            moveVelocity.x -= subtractor.x;
            moveVelocity.y -= subtractor.y;
            moveVelocity.z -= subtractor.z;
            if (moveVelocity.magnitude < 0.5f)
                moveVelocity = Vector3.zero;
        }

        // Collision
        if (moveVelocity.magnitude > 0.2f)
        {
            Ray ray = new Ray(Model.transform.position, moveVelocity.normalized);
            Physics.RaycastNonAlloc(ray, hits, CollisionDistance * moveVelocity.magnitude / 12f,
                    GroundLayers.layers, QueryTriggerInteraction.Ignore);
            if (hits[0].collider)
            {
                OnKnockbackCollision?.Invoke(moveVelocity);
                moveVelocity = Vector3.ProjectOnPlane(moveVelocity, hits[0].normal);
            }

            // Movement
            transform.localPosition += moveVelocity * Time.deltaTime;
        }
}

    public override void ReceiveForce(Vector3 force, bool sticky = false)
    {
        if (sticky && !(Vector3.Dot(force, moveVelocity) < 0f || force.magnitude > moveVelocity.magnitude))
            return;

        force.x /= 2 * Weight;
        force.y /= 2 * Weight;
        force.z /= 2 * Weight;
        moveVelocity.x += force.x;
        moveVelocity.y += force.y;
        moveVelocity.z += force.z;

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
