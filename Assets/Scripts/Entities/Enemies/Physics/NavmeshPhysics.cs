using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshPhysics : IEnemyPhysics
{
    [SerializeField]
    [Tooltip("If true, lets the NavMeshAgent control the rotation of this enemy")]
    protected bool TurnToMoveDirection = false;

    NavMeshAgent pathAgent;

    RaycastHit[] hits = new RaycastHit[1];

    private void Start()
    {
        pathAgent = GetComponent<NavMeshAgent>();
        if (pathAgent)
        {
            Physics.Raycast(new Ray(transform.position + Vector3.up, Vector3.down), out RaycastHit hitInfo, HoverHeight + 1f,
                GroundLayers.layers, QueryTriggerInteraction.Ignore);
            if (hitInfo.collider == null)
                pathAgent.updatePosition = false;
            pathAgent.updateRotation = TurnToMoveDirection;
            pathAgent.updateUpAxis = false;
        }
    }

    public override RaycastHit RayToGround()
    {
        Ray ray;
        if (!pathAgent.updatePosition)
            ray = new Ray(transform.position + Vector3.up, Vector3.down);
        else
            ray = new Ray(pathAgent.nextPosition + Vector3.up, Vector3.down);
        Physics.RaycastNonAlloc(ray, hits, HoverHeight + 1f, GroundLayers.layers, QueryTriggerInteraction.Ignore);
        return hits[0];
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

            if (!pathAgent.enabled)
            {
                pathAgent.enabled = true;
                pathAgent.Warp(transform.position);
            }

            // Slowdown
            if (!pathAgent.updatePosition)
                moveVelocity = Vector3.MoveTowards(moveVelocity, Vector3.zero, Mathf.Max(1f, moveVelocity.magnitude) * Time.deltaTime);
        }
    }


    protected override void FeelKnockback()
    {
        if (!pathAgent.enabled || !pathAgent.updatePosition)
        {
            // Collision
            if (moveVelocity.magnitude > 0.2f) { 
                    Ray ray = new Ray(Model.transform.position, moveVelocity.normalized);
                Physics.RaycastNonAlloc(ray, hits, CollisionDistance * moveVelocity.magnitude / 12f,
                    GroundLayers.layers, QueryTriggerInteraction.Ignore);
            }
            if (hits[0].collider)
            {
                OnKnockbackCollision?.Invoke(moveVelocity);
                moveVelocity = Vector3.zero;
            }

            // Movement
            if (moveVelocity == Vector3.zero)
            {
                pathAgent.updatePosition = true;
                pathAgent.Warp(transform.position);
            }
            else
            {
                transform.position += moveVelocity * Time.deltaTime;
            }
        }
    }


    public override void Stop()
    {
        if (pathAgent && pathAgent.isOnNavMesh)
            pathAgent.isStopped = true;
    }


    public override void ReceiveForce(Vector3 force, bool sticky = false)
    {
        if (sticky && !(Vector3.Dot(force, moveVelocity) < 0f || force.magnitude > moveVelocity.magnitude))
            return;

        moveVelocity += force / (2 * Weight);

        // Stop on the ground
        if (moveVelocity.y < 0f && RayToGround().collider)
            moveVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);

        pathAgent.updatePosition = false;
    }

    public override void WarpPosition(Vector3 newPosition)
    {
        airborne = true;
        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        Ray ray;
        if (!pathAgent)
            ray = new Ray(transform.position, Vector3.down);
        else
            ray = new Ray(pathAgent.nextPosition + Vector3.up, Vector3.down);
            //Ray ray = new Ray(Model.transform.position, Vector3.down);
        Gizmos.DrawRay(ray);
    }
}
