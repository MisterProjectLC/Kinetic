using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    public Transform Root;
    public Transform Tip;
    public GameObject Sparks;

    [Header("Attributes")]
    public float MoveSpeed = 5f;
    public float MaxLifetime = 2f;
    public float Radius = 20f;
    public Color RadiusColor;

    private float clock = 0f;
    private Vector3 moveVelocity;
    private Vector3 m_LastRootPosition;
    private LayerMask hitLayers;

    public void Setup(Vector3 moveDirection, LayerMask layerMask, GameObject sparks)
    {
        clock = 0f;
        hitLayers = layerMask;
        moveVelocity = moveDirection.normalized* MoveSpeed;
        transform.right = moveDirection.normalized;
        Sparks = sparks;

        m_LastRootPosition = Root.position;
    }

    private void OnEnable()
    {
        clock = 0f;
    }

    // Update is called once per frame
    void Update()
    {
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
                OnHit(closestHit.collider);
        }

        m_LastRootPosition = Root.position;
    }


    private bool isHitValid(RaycastHit hit)
    {
        return true;
    }


    private void OnHit(Collider collider)
    {
        GetComponent<Attack>().InflictDamage(collider.gameObject);
        GameObject newObject = ObjectManager.OM.SpawnObjectFromPool(ObjectManager.PoolableType.LaserSparks, Sparks).gameObject;
        newObject.transform.position = transform.position;
        ObjectManager.OM.EraseObject(GetComponent<Poolable>());
    }


    void OnDrawGizmos()
    {
        Gizmos.color = RadiusColor;
        Gizmos.DrawSphere(transform.position, Radius);
    }
}
