using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : EnemyNavigation
{
    [SerializeField]
    float maxDistance = 15f;

    override protected void SetDestination()
    {
        Vector2 randomDirection = Random.insideUnitCircle;
        pathAgent.SetDestination(transform.position + maxDistance* new Vector3(randomDirection.x, 0f, randomDirection.y));
    }
}
