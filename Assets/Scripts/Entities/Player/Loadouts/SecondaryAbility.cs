using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SecondaryAbility : Ability
{
    [HideInInspector]
    public Ability ParentAbility = null;
}
