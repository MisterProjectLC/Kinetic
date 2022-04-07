using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SecondaryAbility : Ability
{
    [HideInInspector]
    public Ability ParentAbility = null;

    protected new void Awake()
    {
        base.Awake();
        Type = LocalizedName.value;
    }
}
