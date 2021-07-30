using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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

    public float Stunned = 0f;

    [Tooltip("How much time it takes to update path")]
    [SerializeField]
    private float updateCooldown = 0.25f;

    float[] clocks = new float[2];
    private NavMeshAgent pathAgent;

    public UnityAction OnActiveUpdate;

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
        if (clocks[0] > weapon.FireCooldown)
        {
            clocks[0] = 0f;
            weapon.Fire();
        }
    }

    private void ManageNavigation()
    {
        if (clocks[1] > updateCooldown)
        {
            clocks[1] = 0f;
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
