using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{  
    [Header("References")]
    [SerializeField]
    Weapon[] weapons;

    [SerializeField]
    public GameObject Model;

    public LayerMask GroundLayers;

    [Header("Attributes")]
    public bool Movable = true;
    public float HoverHeight = 1f;
    float fallSpeed = 0f;

    public float Stunned = 0f;

    [Tooltip("How much time it takes to update path")]
    [SerializeField]
    private float updateCooldown = 0.25f;

    bool airborne = false;
    private Vector3 knockbackForce;
    float navClock = 0f;
    float[] weaponClocks;
    private NavMeshAgent pathAgent;

    public UnityAction OnActiveUpdate;

    void Start()
    {
        weaponClocks = new float[weapons.Length];
        for (int i = 0; i < weaponClocks.Length; i++)
            weaponClocks[i] = 0f;

        pathAgent = GetComponent<NavMeshAgent>();
        if (pathAgent) {
            pathAgent.updateRotation = false;
            pathAgent.updateUpAxis = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FeelKnockback();
        FeelGravity();

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
        navClock += Time.deltaTime;
        if (navClock > updateCooldown)
        {
            navClock = 0f;
            Transform cameraTransform = ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform;
            if (pathAgent.isOnNavMesh)
            {
                pathAgent.isStopped = false;
                pathAgent.SetDestination(cameraTransform.position);
            }
        }
    }


    private void FeelKnockback()
    {
        if (pathAgent && !pathAgent.updatePosition)
        {
            // Collision
            Ray ray = new Ray(transform.position, knockbackForce);
            Physics.Raycast(ray, out RaycastHit hitInfo, 0.5f, GroundLayers, QueryTriggerInteraction.Ignore);
            if (hitInfo.collider)
                knockbackForce = Vector3.zero;

            // Movement
            transform.position += knockbackForce * Time.deltaTime;
            if (knockbackForce == Vector3.zero)
            {
                pathAgent.updatePosition = true;
                pathAgent.Warp(transform.position);
            }
        }
    }


    private void FeelGravity()
    {
        if (!Movable || !airborne)
            return;

        // Air
        RaycastHit hit = RayToGround();
        if (hit.collider == null)
        {
            fallSpeed += Constants.Gravity * Time.deltaTime;
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            Debug.Log("Flying: " + gameObject.name);
        }

        // Ground
        else
        {
            airborne = false;
            fallSpeed = 0f;
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            if (pathAgent)
            {
                if (!pathAgent.enabled)
                    pathAgent.enabled = true;

                if (!pathAgent.updatePosition)
                {
                    if (knockbackForce.y != 0f)
                        knockbackForce = new Vector3(knockbackForce.x, 0f, knockbackForce.z);
                    knockbackForce = Vector3.MoveTowards(knockbackForce, Vector3.zero, Mathf.Max(1f, knockbackForce.magnitude) * Time.deltaTime);
                }
            }
        }
    }


    public RaycastHit RayToGround()
    {
        Ray ray;
        if (!pathAgent)
            ray = new Ray(transform.position, Vector3.down);
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

        knockbackForce = knockback;
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
            Ray ray = new Ray(GetComponent<NavMeshAgent>().nextPosition, Vector3.down);
            //Ray ray = new Ray(Model.transform.position, Vector3.down);
            Gizmos.DrawRay(ray);
        }
    }
}
