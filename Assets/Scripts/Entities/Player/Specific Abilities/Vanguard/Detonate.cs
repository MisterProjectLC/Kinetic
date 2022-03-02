using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : SecondaryAbility
{
    [SerializeField]
    GameObject bigExplosion;

    public override void Execute(Input input)
    {
        int control = 1000;
        List<GameObject> projectilesList = ((ProjectileWeapon)((WeaponAbility)ParentAbility).WeaponRef).ActiveProjectiles;

        while (projectilesList.Count > 0 && control > 0)
        {
            control--;
            projectilesList[0].GetComponent<AttackProjectile>().SetImpactObject(bigExplosion);
            projectilesList[0].GetComponent<AttackProjectile>().Detonate();
        }

        ((WeaponAbility)ParentAbility).ResetCooldown();
    }
}
