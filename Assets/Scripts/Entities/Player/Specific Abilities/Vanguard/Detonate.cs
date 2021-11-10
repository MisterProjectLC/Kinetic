using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : SecondaryAbility
{
    public override void Execute(Input input)
    {
        int control = 1000;

        while (((ProjectileWeapon)((WeaponAbility)ParentAbility).WeaponRef).ActiveProjectiles.Count > 0 && control > 0)
        {
            control--;
            ((ProjectileWeapon)((WeaponAbility)ParentAbility).WeaponRef).ActiveProjectiles[0].GetComponent<AttackProjectile>().Detonate();
        }

        ((WeaponAbility)ParentAbility).ResetCooldown();
    }
}
