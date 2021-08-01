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
    public UnityAction<Collider> OnHit;

    public void Setup(Vector3 moveDirection, LayerMask layerMask)
    {
        clock = 0f;
        hitLayers = layerMask;
        moveVelocity = moveDirection.normalized* MoveSpeed;
        transform.right = moveDirection.normalized;

        m_LastRootPosition = Root.position;
    }

    private void OnEnable()
    {
        clock = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GravitySpeed != 0f)
            moveVelocity += Vector3.down * GravitySpeed * Time.deltaTime;

        // Bullet lifetime
        transform.position += moveVelocity * Time.deltaTime;
        clock += Time.deltaTime;
        if (clock > MaxLifetime)
            ObjectManager.OM.EraseObject(GetComponent<Poolable>());

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
