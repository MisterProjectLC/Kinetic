using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Attributes")]
    [Tooltip("Explosion range")]
    public float Radius = 5f;

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

    Attack attack;
    [SerializeField]
    const int maxColliders = 20;
    Collider[] colliders = new Collider[maxColliders];
    List<Health> enemies = new List<Health>(maxColliders);

    Vector3? vectorToShield = null;

    // Start is called before the first frame update
    void Awake()
    {
        attack = GetComponent<Attack>();
    }

    private void OnEnable()
    {
        colliders = new Collider[maxColliders];
        enemies.Clear();
        StartCoroutine("Explode");
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.01f);
        // Emit sound
        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();

        Physics.OverlapSphereNonAlloc(transform.position, Radius, colliders, HitLayers.layers);
        foreach (Collider collider in colliders)
        {
            if (!collider)
                break;

            float rate = ((NeuteredHitLayers.value >> collider.gameObject.layer) == 1) ? NeuteredRate : 1f;
            float distanceToTarget = (transform.position - collider.ClosestPoint(transform.position)).magnitude / Radius;

            if (collider.GetComponent<Damageable>() && !enemies.Contains(collider.GetComponent<Damageable>().GetHealth()))
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Player Shield"))
                    vectorToShield = collider.transform.position - transform.position;
                else if (collider.gameObject.layer == LayerMask.NameToLayer("Player") && vectorToShield != null) {
                    Vector3 vectorToPlayer = collider.transform.position - transform.position;
                    if (Vector3.Dot(vectorToShield.Value, vectorToPlayer) > 0 && vectorToShield.Value.magnitude < vectorToPlayer.magnitude)
                        continue;
                }
                enemies.Add(collider.GetComponent<Damageable>().GetHealth());
                attack.AttackTarget(collider.gameObject, rate * (1f - distanceToTarget * (1f - FallOffRate)));
            }
        }
    }
}