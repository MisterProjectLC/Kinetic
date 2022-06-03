using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : EnemyNavigation
{
    override protected void SetDestination()
    {
        Transform cameraTransform = enemy.PlayerTransform.GetComponentInChildren<Camera>().transform;
        pathAgent.SetDestination(cameraTransform.position);
    }
}
