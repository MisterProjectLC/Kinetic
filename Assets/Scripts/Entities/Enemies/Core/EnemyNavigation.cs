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

    [SerializeField]
    bool OnlyIfPlayerInView = true;

    protected Clock navClock;
    protected NavMeshAgent pathAgent;
    protected Enemy enemy;

    private void Start()
    {
        navClock = new Clock(updateCooldown);
        pathAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        enemy.SubscribeToUpdate(ManageNavigation);
    }

    private void ManageNavigation()
    {
        if (!pathAgent || enemy.HasAnyOfTheseStatusEffects(blockingEffects) || (OnlyIfPlayerInView && !enemy.IsPlayerInView()))
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
