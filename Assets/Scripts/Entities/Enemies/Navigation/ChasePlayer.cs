using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : EnemyNavigation
{
    override protected void SetDestination()
    {
        Transform cameraTransform = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
        pathAgent.SetDestination(cameraTransform.position);
    }
}
