using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSlot : LoadoutSlot
{
    private void Start()
    {
        Type = "Passive";
    }

    public override void SetOptionPrivate(Upgrade option, bool activating)
    {
        loadoutManager.SetPassive(option, activating);
    }

    protected override int GetLoadoutNumber()
    {
        return -1;
    }
}
