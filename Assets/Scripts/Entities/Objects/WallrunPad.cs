using System.Collections.Generic;
using UnityEngine;

public class WallrunPad : Pad
{
    WallRun wallrun;
    bool active = false;
    float timeSinceLastDetach = 0f;

    new void Update()
    {
        timeSinceLastDetach += Time.deltaTime;
        base.Update();
    }

    protected override void ApplyEffect(GameObject target)
    {
        if (active || timeSinceLastDetach < 1f)
            return;

        active = true;
        wallrun = target.GetComponentInChildren<WallRun>(true);
        if (!wallrun)
            return;

        wallrun.UnsubscribeToDetach(OnDetach);
        wallrun.gameObject.SetActive(true);
        wallrun.SubscribeToDetach(OnDetach);
    }

    void OnDetach()
    {
        timeSinceLastDetach = 0f;
        active = false;
        wallrun.gameObject.SetActive(false); 
    }
}
