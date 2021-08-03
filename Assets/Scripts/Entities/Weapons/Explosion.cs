using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Radius = 5f;
    public LayerMask HitLayers;
    //public float KnockbackForce = 5f;

    Attack attack;
    [SerializeField]
    const int maxColliders = 20;
    Collider[] colliders = new Collider[maxColliders];

    // Start is called before the first frame update
    void Awake()
    {
        attack = GetComponent<Attack>();
    }

    private void OnEnable()
    {
        StartCoroutine("Explode");
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.01f);
        Physics.OverlapSphereNonAlloc(transform.position, Radius, colliders, HitLayers);
        foreach (Collider collider in colliders)
        {
            if (!collider)
                break;

            attack.AttackTarget(collider.gameObject);
        }
    }
}