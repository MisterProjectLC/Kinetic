using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyNavigation : MonoBehaviour
{
    [Tooltip("How much time it takes to update path")]
    [SerializeField]
    protected float updateCooldown = 0.25f;

    protected float navClock = 0f;
    protected NavMeshAgent pathAgent;


    private void Start()
    {
        navClock = updateCooldown;
        pathAgent = GetComponent<NavMeshAgent>();
        GetComponent<Enemy>().OnActiveUpdate += ManageNavigation;
    }

    private void ManageNavigation()
    {
        if (!pathAgent)
            return;

        navClock += Time.deltaTime;
        if (navClock > updateCooldown)
        {
            navClock = 0f;
            if (pathAgent.isOnNavMesh)
            {
                pathAgent.isStopped = false;
                SetDestination();
            }
        }
    }

    abstract protected void SetDestination();
}
