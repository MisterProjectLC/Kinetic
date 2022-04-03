using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    [Header("Attributes")]
    [Tooltip("Explosion range")]
    public float Radius = 5f;

    [Range(0f, 1f)]
    [Tooltip("Percentage damage at the radius' center")]
    public float CenterRate = 1f;

    [Range(0f, 1f)]
    [Tooltip("Percentage damage at the radius' edge")]
    public float FallOffRate = 1f;

    [Range(0f, 1f)]
    [Tooltip("Percentage damage against neutered layers")]
    public float NeuteredRate = 1f;

    [Header("Hit layers")]
    public LayersConfig HitLayers;
    public LayerMask NeuteredHitLayers;
    //public float KnockbackForce = 5f;

    Attack[] attacks;
    [SerializeField]
    const int maxColliders = 150;
    Collider[] colliders = new Collider[maxColliders];
    AreaAttack areaAttack;
    List<AreaAttack.CollisionData> enemies = new List<AreaAttack.CollisionData>(30);

    [SerializeField]
    bool GetClosestPoint = true;

    Vector3? vectorToShield = null;

    // Start is called before the first frame update
    void Awake()
    {
        attacks = GetComponents<Attack>();
        areaAttack = new AreaAttack(transform);
    }

    private void OnEnable()
    {
        colliders = new Collider[maxColliders];
        enemies.Clear();
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.01f);
        // Emit sound
        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();

        Physics.OverlapSphereNonAlloc(transform.position, Radius, colliders, HitLayers.layers);
        // Get the closest collider of each health entity
        enemies = areaAttack.RefineHits(colliders);

        // Calculate and apply the damage
        foreach (AreaAttack.CollisionData collision in enemies)
        {
            Collider collider = collision.closestCollider;

            float rate = ((NeuteredHitLayers.value >> collider.gameObject.layer) == 1) ? NeuteredRate : 1f;
            Vector3 colliderPoint = GetClosestPoint ? collider.ClosestPoint(transform.position) : collider.transform.position;
            float distanceToTarget = (transform.position - colliderPoint).magnitude / Radius;

            if (collider.gameObject.layer == LayerMask.NameToLayer("Player Shield"))
                vectorToShield = collider.transform.position - transform.position;

            else if (collider.gameObject.layer == LayerMask.NameToLayer("Player") && vectorToShield != null) {
                Vector3 vectorToPlayer = collider.transform.position - transform.position;
                if (Vector3.Dot(vectorToShield.Value, vectorToPlayer) > 0 && vectorToShield.Value.magnitude < vectorToPlayer.magnitude)
                    continue;
            }

            foreach (Attack attack in attacks)
                attack.AttackTarget(collider.gameObject, rate * Mathf.Lerp(CenterRate, FallOffRate, distanceToTarget));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, Radius);
    }
}