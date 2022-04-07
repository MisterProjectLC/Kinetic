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

        Physics.SphereCastNonAlloc(transform.position, 2f, transform.forward, hitInfos, 3f, sphereCastLayers.value, QueryTriggerInteraction.Collide);
        RaycastHit hitInfo = hitInfos[0];
        if (hitInfo.collider) {
            Transform centibomb = hitInfo.collider.transform.parent;
            if (hitInfo.collider.transform.parent && hitInfo.collider.transform.parent.GetComponent<Centibomb>() &&
                centibomb.name.StartsWith("Centibomb") && !siblings.Contains(centibomb.GetComponent<Centibomb>()))
                health.Kill();

            else if (hitInfo.collider.name != "EN_Centobomba" && (!centibomb || centibomb.name != "EN_Centobomba"))
            {
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
