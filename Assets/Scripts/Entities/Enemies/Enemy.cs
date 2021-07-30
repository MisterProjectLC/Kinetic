using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{  
    [Header("References")]
    [SerializeField]
    Weapon weapon;

    public LayerMask GroundLayers;

    [Header("Attributes")]
    public bool Movable = true;
    public float Height = 1f;
    float fallSpeed = 0f;

    [Tooltip("How much time it takes to update path")]
    [SerializeField]
    private float updateCooldown = 0.25f;

    float[] clocks = new float[2];
    private NavMeshAgent pathAgent;


    void Start()
    {
        for (int i = 0; i < clocks.Length; i++)
            clocks[i] = 0f;
        pathAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < clocks.Length; i++)
            clocks[i] += Time.deltaTime;

        // Weapon
        if (clocks[0] > weapon.FireCooldown)
        {
            clocks[0] = 0f;
            weapon.Fire();
        }

        // Navigation
        if (clocks[1] > updateCooldown)
        {
            clocks[1] = 0f;
            Transform cameraTransform = ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform;
            if (pathAgent.isOnNavMesh)
                pathAgent.SetDestination(cameraTransform.position);
        }

        // Gravity
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
}
