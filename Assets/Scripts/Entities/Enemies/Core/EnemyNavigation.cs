using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyNavigation : MonoBehaviour
{
    [Tooltip("How much time it takes to update path")]
    [SerializeField]
    protected float updateCooldown = 0.25f;

    [SerializeField]
    List<StatusEffect> blockingEffects;


    protected Clock navClock;
    protected NavMeshAgent pathAgent;
    Enemy enemy;

    private void Start()
    {
        navClock = new Clock(updateCooldown);
        pathAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        enemy.SubscribeToUpdate(ManageNavigation);
    }

    private void ManageNavigation()
    {
        if (!pathAgent || enemy.HasAnyOfTheseStatusEffects(blockingEffects))
            return;

        if (navClock.TickAndRing(Time.deltaTime))
        {
            if (pathAgent.isOnNavMesh)
            {
                pathAgent.isStopped = false;
                SetDestination();
            }
        }
    }

    abstract protected void SetDestination();
}
