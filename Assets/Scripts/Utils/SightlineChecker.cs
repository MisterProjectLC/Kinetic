using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightlineChecker
{
    Transform watcher;
    RaycastHit[] hits;
    LayerMask viewBlockedLayers;
    float distanceOffset = 5f;

    public SightlineChecker(Transform watcher, LayerMask viewBlockedLayers, float distanceOffset = 0f)
    {
        hits = new RaycastHit[5];
        this.watcher = watcher;
        this.viewBlockedLayers = viewBlockedLayers;
        this.distanceOffset = distanceOffset;
    }

    public bool IsTargetInWatchersView(Transform target)
    {
        if (target == null)
            return true;

        Vector3 playerDistance = target.position - watcher.position;

        // Check if view is obstructed
        Ray ray = new Ray(watcher.position, playerDistance.normalized);
        if (Physics.RaycastNonAlloc(ray, hits, 500f, viewBlockedLayers, QueryTriggerInteraction.Ignore) == 0)
            return false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider && (hit.distance+ distanceOffset < playerDistance.magnitude))
                return false;
        }

        return true;
    }
}
