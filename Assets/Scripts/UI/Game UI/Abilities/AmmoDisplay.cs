using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : IAbilityAdditionalView
{
    [SerializeField]
    Text AmmoLabel;

    Ability ability;

    public override void Setup(Ability ability)
    {
        this.ability = ability;
        UpdateAmmo(ability);
        ability.SubscribeToExecuteAbility(UpdateAmmo);
    }

    private void OnDestroy()
    {
        ability.UnsubscribeToExecuteAbility(UpdateAmmo);
    }


    void UpdateAmmo(Ability ability)
    {
        Weapon weapon = ((WeaponAbility)ability).WeaponRef;
        string text = weapon.Ammo.ToString() + "/" + weapon.MaxAmmo.ToString();
        AmmoLabel.text = text;
    }
}
