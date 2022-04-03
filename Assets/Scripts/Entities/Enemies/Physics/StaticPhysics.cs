using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPhysics : IEnemyPhysics
{

    public override RaycastHit RayToGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hitInfo, HoverHeight + 1f, GroundLayers.layers, QueryTriggerInteraction.Ignore);
        return hitInfo;
    }

    protected override void FeelGravity()
    {
        return;
    }


    protected override void FeelKnockback()
    {
        return;
    }


    public override void Stop()
    {
        return;
    }


    public override void ReceiveForce(Vector3 force, bool sticky = false)
    {
        return;
    }

    public override void WarpPosition(Vector3 newPosition)
    {
        return;
    }
}
