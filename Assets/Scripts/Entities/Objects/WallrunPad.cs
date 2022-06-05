using System.Collections.Generic;
using UnityEngine;

public class WallrunPad : Pad
{
    GameObject currentTarget;

    protected override void ApplyEffect(GameObject target)
    {
        if (currentTarget == target)
            return;

        currentTarget = target;
        target.GetComponentInChildren<WallRun>().AttachToWall();
    }
}
