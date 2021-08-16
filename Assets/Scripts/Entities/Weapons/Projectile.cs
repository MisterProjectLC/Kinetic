using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    public Transform Root;
    public Transform Tip;

    [Header("Attributes")]
    public float MoveSpeed = 5f;
    public float GravitySpeed = 0f;
    public float MaxLifetime = 2f;
    public float Radius = 20f;
    public Color RadiusColor;

    private float clock = 0f;
    private Vector3 moveVelocity;
    private Vector3 m_LastRootPosition;
    private LayerMask hitLayers;

    [HideInInspector]
    public bool KeepAlive;
    [HideInInspector]
    public bool Stopped;

    [HideInInspector]
    public UnityAction<Collider> OnHit;

    [HideInInspector]
    public GameObject Shooter;

    public void Setup(Vector3 moveDirection, LayerMask layerMask)
    {
        Setup(moveDirection, layerMask, null);
    }

    public void Setup(Vector3 moveDirection, LayerMask layerMask, GameObject shooter)
    {
        clock = 0f;
        KeepAlive = false;
        Stopped = false;
        hitLayers = layerMask;
        moveVelocity = moveDirection.normalized * MoveSpeed;
        transform.right = moveDirection.normalized;
        Shooter = shooter;

        m_LastRootPosition = Root.position;
    }

    private void OnEnable()
    {
        clock = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        if (GravitySpeed != 0f)
            moveVelocity += Vector3.down * GravitySpeed * Time.deltaTime;
        if (!Stopped)
            transform.position += moveVelocity * Time.deltaTime;

        // Bullet lifetime
        if (!KeepAlive)
        {
            clock += Time.deltaTime;
            if (clock > MaxLifetime)
            {
                if (GetComponent<Poolable>())
                    ObjectManager.OM.EraseObject(GetComponent<Poolable>());
                else
                    gameObject.SetActive(false);
            }
        }

        // Hit detection
        {
            RaycastHit closestHit = new RaycastHit();
            closestHit.distance = Mathf.Infinity;
            bool foundHit = false;

            // Sphere cast
            Vector3 displacementSinceLastFrame = Tip.position - m_LastRootPosition;
            RaycastHit[] hits = Physics.SphereCastAll(m_LastRootPosition, Radius,
                displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude, hitLayers, QueryTriggerInteraction.Ignore);
            foreach (var hit in hits)
            {
                if (isHitValid(hit) && hit.distance < closestHit.distance)
                {
                    foundHit = true;
                    closestHit = hit;
                }
            }

            if (foundHit)              
                OnHit.Invoke(closestHit.collider);
        }

        m_LastRootPosition = Root.position;
    }


    private bool isHitValid(RaycastHit hit)
    {
        return true;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = RadiusColor;
        Gizmos.DrawSphere(transform.position, Radius);
    }
}
