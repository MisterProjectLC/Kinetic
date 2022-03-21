using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : PhysicsEntity
{
    [SerializeField]
    bool debug = false;

    [Header("References")]
    [SerializeField]
    public Weapon[] weapons;

    [SerializeField]
    public GameObject Model;

    public LayersConfig GroundLayers;
    public LayersConfig ViewBlockedLayers;

    [Header("Attributes")]
    public bool Movable = true;

    [Header("Options")]
    [SerializeField]
    [Tooltip("If true, lets the NavMeshAgent control the rotation of this enemy")]
    bool TurnToMoveDirection = false;

    public bool OnlyShootIfPlayerInView = true;

    [SerializeField]
    float CollisionDistance = 1f;

    Vector3 moveVelocity = Vector3.zero;

    [HideInInspector]
    public float Stunned = 0f;
    float paralyzed = 0f;

    bool airborne = true;
    private NavMeshAgent pathAgent;
    Transform playerTransform;

    UnityAction OnUnstunnedUpdate;
    public void SubscribeToUnstunnedUpdate(UnityAction subscriber) { OnUnstunnedUpdate += subscriber; }
    UnityAction OnActiveUpdate;
    public void SubscribeToActiveUpdate(UnityAction subscriber) { OnActiveUpdate += subscriber; }
    public UnityAction<Vector3> OnKnockbackCollision;

    private void Awake()
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

    void Start()
    {
        playerTransform = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    override protected void MyUpdate()
    {
        FeelGravity();
        FeelKnockback();

        OnUnstunnedUpdate?.Invoke();

        if (Stunned <= 0f)
        {
            ActivateWeapons();
            if (paralyzed <= 0f)
            {
                OnActiveUpdate?.Invoke();
            }
            else
            {
                paralyzed -= Time.deltaTime;
                if (paralyzed <= 0f && pathAgent)
                    pathAgent.isStopped = true;
            }
        }
        else
        {
            Stunned -= Time.deltaTime;
            if (Stunned <= 0f)
                GravityMultiplier = lastGravityMultiplier;
        }
    }


    private void ActivateWeapons()
    {
        if (OnlyShootIfPlayerInView && !IsPlayerInView())
            return;

        foreach (Weapon weapon in weapons)
            if (weapon)
                weapon.Trigger();
    }

    public bool IsPlayerInView()
    {
        Vector3 playerDistance = playerTransform.position - Model.transform.position;

        // Check if view is obstructed
        Ray ray = new Ray(Model.transform.position, playerDistance.normalized);
        Physics.Raycast(ray, out RaycastHit hit, 500f, ViewBlockedLayers.layers, QueryTriggerInteraction.Ignore);
        if (hit.collider && (hit.distance < playerDistance.magnitude))
            return false;

        return true;
    }


    override protected void FeelGravity()
    {
        if (!Movable)
            return;
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

            if (pathAgent)
            {
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
    }


    override protected void FeelKnockback()
    {
        // Automatic desacceleration (for airborne enemies)
        if (AirDesacceleration > 0 && moveVelocity.magnitude > 0)
        {
            moveVelocity -= moveVelocity.normalized * AirDesacceleration * Time.deltaTime;
            if (moveVelocity.magnitude < 0.5f)
                moveVelocity = Vector3.zero;
        }

        if (!pathAgent || !pathAgent.enabled || !pathAgent.updatePosition)
        {
            // Collision
            Ray ray = new Ray(Model.transform.position, moveVelocity.normalized);
            Physics.Raycast(ray, out RaycastHit hitInfo, CollisionDistance*moveVelocity.magnitude/12f, 
                GroundLayers.layers, QueryTriggerInteraction.Ignore);
            if (hitInfo.collider)
            {
                OnKnockbackCollision?.Invoke(moveVelocity);
                moveVelocity = Vector3.zero;
            }

            // Movement
            if (moveVelocity == Vector3.zero)
            {
                if (pathAgent)
                {
                    pathAgent.updatePosition = true;
                    pathAgent.Warp(transform.position);
                }
            }
            else
            {
                transform.position += moveVelocity * Time.deltaTime;
            }
        }
    }


    public RaycastHit RayToGround()
    {
        //Debug.Log(pathAgent.updatePosition);

        Ray ray;
        if (!pathAgent || !pathAgent.updatePosition)
            ray = new Ray(transform.position + Vector3.up, Vector3.down);
        else
            ray = new Ray(pathAgent.nextPosition + Vector3.up, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hitInfo, HoverHeight + 1f, GroundLayers.layers, QueryTriggerInteraction.Ignore);
        return hitInfo;
    }


    public void ReceiveStun(float duration)
    {
        Stunned = duration;
        if (GravityMultiplier < 1f)
        {
            lastGravityMultiplier = GravityMultiplier;
            GravityMultiplier = 1f;
        }
        if (pathAgent && pathAgent.isOnNavMesh)
            pathAgent.isStopped = true;
    }

    public void ReceiveParalyze(float duration)
    {
        paralyzed = duration;
        if (pathAgent && pathAgent.isOnNavMesh)
            pathAgent.isStopped = true;
    }

    override public void ReceiveKnockback(Vector3 knockback)
    {
        if (!Movable)
            return;

        moveVelocity += knockback / (2 * Weight);
        //Debug.Log("MV: " + moveVelocity);

        // Stop on the ground
        if (moveVelocity.y < 0f)
            moveVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);

        if (pathAgent)
            pathAgent.updatePosition = false;
    }

    override public void WarpPosition(Vector3 newPosition)
    {
        airborne = true;
        transform.position = newPosition;
    }

    override public Vector3 GetMoveVelocity()
    {
        return moveVelocity;
    }

    public float GetCollisionDistance()
    {
        return CollisionDistance;
    }

    private void OnDrawGizmos()
    {
        if (GetComponent<NavMeshAgent>())
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
}
