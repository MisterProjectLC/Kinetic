using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Centibomb : MonoBehaviour
{
    [SerializeField]
    LayerMask sphereCastLayers;

    static float MAX_EXPLOSION_DISTANCE = 15f;
    List<Centibomb> siblings = new List<Centibomb>();

    [SerializeField]
    bool leader = false;

    float killTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NavMeshAgent>().destination = transform.position;
        GetComponent<Enemy>().OnKnockbackCollision += Detonate;

        if (leader)
        {
            List<Centibomb> children = new List<Centibomb>(transform.GetComponentsInChildren<Centibomb>());
            foreach (Centibomb c in children)
            {
                c.siblings = children;
                c.transform.SetParent(transform.parent);
            }
            siblings = children;
        }
    }


    private void Update()
    {
        if (!leader && (!GetComponent<ChaseTarget>().Target ||
            (GetComponent<ChaseTarget>().Target.position - transform.position).magnitude > MAX_EXPLOSION_DISTANCE))
        {
            killTimer -= Time.deltaTime;
            if (killTimer < 0f)
                GetComponent<Health>().Kill();
        }
    
    }

    private void FixedUpdate()
    {
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, 1.5f, transform.forward, out hitInfo, 1f, sphereCastLayers.value, QueryTriggerInteraction.Collide);
        if (hitInfo.collider && hitInfo.collider.transform.parent && hitInfo.collider.transform.parent.GetComponent<Centibomb>())
        {
            Transform centibomb = hitInfo.collider.transform.parent;

            if (centibomb.name.StartsWith("Centibomb")) 
                if (!siblings.Contains(centibomb.GetComponent<Centibomb>()))
                    GetComponent<Health>().Kill();
        }
    }

    void Detonate(Vector3 moveVelocity)
    {
        if (moveVelocity.magnitude > 2f)
            GetComponent<Health>().Kill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            GetComponent<Health>().Kill();
    }
}
