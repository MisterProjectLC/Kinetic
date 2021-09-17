using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : SecondaryAbility
{
    public override void Execute(Input input)
    {
        while (((WeaponAbility)ParentAbility).WeaponRef.ActiveProjectiles.Count > 0)
        {
            ((WeaponAbility)ParentAbility).WeaponRef.ActiveProjectiles[0].GetComponent<AttackProjectile>().Detonate();
        }

        ((WeaponAbility)ParentAbility).ResetCooldown();
    }
}
