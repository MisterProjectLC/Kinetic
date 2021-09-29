using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    bool debug = false;

    [Header("References")]
    [SerializeField]
    Weapon[] weapons;

    [SerializeField]
    public GameObject Model;

    public LayerMask GroundLayers;

    [Header("Attributes")]
    public bool Movable = true;
    public float HoverHeight = 1f;
    public float GravityMultiplier = 1f;
    public float AirDesacceleration = 0f;

    Vector3 moveVelocity = Vector3.zero;

    [HideInInspector]
    public float Stunned = 0f;

    [Tooltip("How much time it takes to update path")]
    [SerializeField]
    private float updateCooldown = 0.25f;

    bool airborne = false;
    //private Vector3 knockbackForce;
    float navClock = 0f;
    float[] weaponClocks;
    private NavMeshAgent pathAgent;

    public UnityAction OnActiveUpdate;

    void Start()
    {
        weaponClocks = new float[weapons.Length];
        for (int i = 0; i < weaponClocks.Length; i++)
            weaponClocks[i] = 0f;

        navClock = updateCooldown;
        pathAgent = GetComponent<NavMeshAgent>();
        if (pathAgent) {
            pathAgent.updateRotation = false;
            pathAgent.updateUpAxis = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FeelGravity();
        FeelKnockback();

        if (Stunned <= 0f)
        {
            ActivateWeapons();
            ManageNavigation();
            if (OnActiveUpdate != null)
                OnActiveUpdate.Invoke();
        }
        else
            Stunned -= Time.deltaTime;
    }


    private void ActivateWeapons()
    {
        int i = 0;
        foreach (Weapon weapon in weapons)
        {
            weaponClocks[i] += Time.deltaTime;
            if (weapon)
                if (weaponClocks[i] > weapon.FireCooldown)
                {
                    weaponClocks[i] = 0f;
                    weapon.Fire();
                }
            i++;
        }
    }

    private void ManageNavigation()
    {
        if (!pathAgent)
            return;

        navClock += Time.deltaTime;
        if (navClock > updateCooldown)
        {
            navClock = 0f;
            Transform cameraTransform = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
            if (pathAgent.isOnNavMesh)
            {
                pathAgent.isStopped = false;
                pathAgent.SetDestination(cameraTransform.position);
            }
        }
    }


    private void FeelGravity()
    {
        if (!Movable)
            return;
        RaycastHit hit = RayToGround();
        airborne = (hit.collider == null);

        // Air
        if (airborne)
            moveVelocity += Vector3.down * GravityMultiplier * Constants.Gravity * Time.deltaTime;

        // Ground
        else
        {
            // Stop on the ground
            if (moveVelocity.y < 0f)
                moveVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);

            if (pathAgent)
            {
                if (!pathAgent.enabled)
                    pathAgent.enabled = true;

                // Slowdown
                if (!pathAgent.updatePosition)
                    moveVelocity = Vector3.MoveTowards(moveVelocity, Vector3.zero, Mathf.Max(1f, moveVelocity.magnitude) * Time.deltaTime);
            }
        }
    }


    private void FeelKnockback()
    {
        // Automatic desacceleration (for airborne enemies)
        if (AirDesacceleration > 0 && moveVelocity.magnitude > 0)
        {
            moveVelocity -= moveVelocity.normalized * AirDesacceleration * Time.deltaTime;
            if (moveVelocity.magnitude < 0.5f)
                moveVelocity = Vector3.zero;
        }

        if (!pathAgent || !pathAgent.updatePosition)
        {
            // Collision
            Ray ray = new Ray(transform.position, moveVelocity);
            Physics.Raycast(ray, out RaycastHit hitInfo, 1f, GroundLayers, QueryTriggerInteraction.Ignore);
            if (hitInfo.collider)
                moveVelocity = Vector3.zero;

            // Movement
            if (moveVelocity == Vector3.zero)
            {
                if (pathAgent)
                {
                    pathAgent.updatePosition = true;
                    pathAgent.Warp(transform.position);
                }
            } else
                transform.position += moveVelocity * Time.deltaTime;
        }
    }


    public RaycastHit RayToGround()
    {
        Ray ray;
        if (!pathAgent || !pathAgent.updatePosition)
            ray = new Ray(transform.position + Vector3.up, Vector3.down);
        else
            ray = new Ray(pathAgent.nextPosition + Vector3.up, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hitInfo, HoverHeight + 1f, GroundLayers, QueryTriggerInteraction.Ignore);
        return hitInfo;
    }


    public void ReceiveStun(float duration)
    {
        Stunned = duration;
        if (pathAgent && pathAgent.isOnNavMesh)
            pathAgent.isStopped = true;
    }

    public void ReceiveKnockback(Vector3 knockback)
    {
        if (!Movable)
            return;

        moveVelocity += (knockback / 2);

        // Parar no chão
        if (moveVelocity.y < 0f)
            moveVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);

        if (pathAgent)
            pathAgent.updatePosition = false;
    }

    public void WarpPosition(Vector3 newPosition)
    {
        airborne = true;
        transform.position = newPosition;
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
