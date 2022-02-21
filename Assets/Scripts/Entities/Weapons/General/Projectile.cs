using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    static float AUTOHIT_WAITTIME = 3f;

    [Header("References")]
    public Transform Root;
    public Transform Tip;

    [Header("Attributes")]
    public ProjectileConfig Attributes;

    private float clock = 0f;
    private Vector3 moveVelocity;
    public Vector3 MoveVelocity { 
        get {
            return moveVelocity;
        } 
        set {
            moveVelocity = value;
        }
    }

    private Vector3 m_LastRootPosition;
    private LayerMask hitLayers;

    [HideInInspector]
    public bool KeepAlive;
    [HideInInspector]
    public bool Stopped;

    [HideInInspector]
    public UnityAction<Collider> OnHit;
    public UnityAction<Projectile> OnDestroy;

    [HideInInspector]
    public GameObject Shooter;

    public void Setup(Vector3 moveDirection, LayerMask layerMask, GameObject shooter = null)
    {
        clock = 0f;
        KeepAlive = false;
        Stopped = false;
        hitLayers = layerMask;
        moveVelocity = moveDirection.normalized * Attributes.MoveSpeed;
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
        if (Attributes.GravitySpeed != 0f)
            moveVelocity += Vector3.down * Attributes.GravitySpeed * Time.deltaTime;
        if (!Stopped)
            transform.position += moveVelocity * Time.deltaTime;

        // Bullet lifetime
        if (!KeepAlive)
        {
            clock += Time.deltaTime;
            if (clock > Attributes.MaxLifetime)
            {
                if (GetComponent<Poolable>())
                    ObjectManager.OM.EraseObject(GetComponent<Poolable>());
                else
                    gameObject.SetActive(false);
            }
        }

        // Hit detection
        if (clock > AUTOHIT_WAITTIME/Attributes.MoveSpeed || KeepAlive)
        {
            RaycastHit closestHit = new RaycastHit();
            closestHit.distance = Mathf.Infinity;
            bool foundHit = false;

            // Sphere cast
            Vector3 displacementSinceLastFrame = Tip.position - m_LastRootPosition;
            RaycastHit[] hits = Physics.SphereCastAll(m_LastRootPosition, Attributes.Radius,
                displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude, hitLayers, QueryTriggerInteraction.Ignore);
            foreach (var hit in hits)
            {
                if (hit.distance < closestHit.distance)
                {
                    foundHit = true;
                    closestHit = hit;
                }
            }

            if (foundHit)              
                OnHit?.Invoke(closestHit.collider);
        }

        m_LastRootPosition = Root.position;
    }


    public void Destroy()
    {
        OnDestroy?.Invoke(this);

        if (GetComponent<Poolable>())
            ObjectManager.OM.EraseObject(GetComponent<Poolable>());
        else
            gameObject.SetActive(false);
    }
}
