using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{  
    [Header("References")]
    [SerializeField]
    Weapon[] weapons;

    public LayerMask GroundLayers;

    [Header("Attributes")]
    public bool Movable = true;
    public float Height = 1f;
    float fallSpeed = 0f;

    public float Stunned = 0f;

    [Tooltip("How much time it takes to update path")]
    [SerializeField]
    private float updateCooldown = 0.25f;

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
        navClock += Time.deltaTime;

        FeelGravity();

        if (Stunned <= 0f)
        {
            ActivateWeapons();
            ManageNavigation();
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
        if (navClock > updateCooldown)
        {
            navClock = 0f;
            Transform cameraTransform = ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform;
            if (pathAgent.isOnNavMesh)
                pathAgent.SetDestination(cameraTransform.position);
        }
    }

    private void FeelGravity()
    {
        if (RayToGround().collider == null)
        {
            fallSpeed += Constants.Gravity * Time.deltaTime;
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
        else
        {
            fallSpeed = 0f;
            if (pathAgent && !pathAgent.enabled)
                pathAgent.enabled = true;
        }
    }


    public RaycastHit RayToGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hitInfo, Height, GroundLayers, QueryTriggerInteraction.Ignore);
        return hitInfo;
    }


    public void ReceiveStun(float duration)
    {
        Stunned = duration;
        if (pathAgent.enabled && pathAgent.isOnNavMesh)
            pathAgent.SetDestination(transform.position);
    }
}
