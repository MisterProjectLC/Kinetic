using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : EnemyNavigation
{
    public Transform Target;

    override protected void SetDestination()
    {
        if (Target)
            pathAgent.SetDestination(Target.position);
    }
}
