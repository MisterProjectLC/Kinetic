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

    Health health;
    ChaseTarget chaseTarget;
    RaycastHit[] hitInfos;
    Clock clock;

    // Start is called before the first frame update
    void Start()
    {
        hitInfos = new RaycastHit[1];
        clock = new Clock(0.8f);
        health = GetComponent<Health>();
        chaseTarget = GetComponent<ChaseTarget>();

        if (GetComponent<NavMeshAgent>().isOnNavMesh)
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
        if (!leader && (!chaseTarget.Target ||
            (chaseTarget.Target.position - transform.position).magnitude > MAX_EXPLOSION_DISTANCE))
        {
            killTimer -= Time.deltaTime;
            if (killTimer < 0f)
                health.Kill();
        }
    
    }

    private void FixedUpdate()
    {
        if (!clock.TickAndRing(Time.deltaTime))
            return;

        RaycastHit[] hitInfos = Physics.SphereCastAll(transform.position, 3f, transform.forward, 5f, sphereCastLayers.value, QueryTriggerInteraction.Collide);
        foreach (RaycastHit hitInfo in hitInfos)
        {
            if (!hitInfo.collider)
                return;

            Centibomb centibomb = hitInfo.collider.GetComponentInParent<Centibomb>();
            if (centibomb)
            {
                if (centibomb.leader && !siblings.Contains(centibomb))
                {
                    Debug.Log("A " + hitInfo.collider.name);
                    health.Kill();
                }
            }
            else
            {
                Debug.Log("B " + hitInfo.collider.name);
                health.Kill();
            }
        }
        
    }

    void Detonate(Vector3 moveVelocity)
    {
        if (moveVelocity.magnitude > 2f)
            health.Kill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            health.Kill();
    }
}
