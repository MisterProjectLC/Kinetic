using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAllAmmo : MonoBehaviour
{
    void Start()
    {
        GetComponent<GameTrigger>().SubscribeToTriggerActivate(RemoveAmmo);
    }

    // Update is called once per frame
    void RemoveAmmo()
    {
        GameObject player = ActorsManager.AM.GetPlayer().gameObject;
        foreach (WeaponAbility weapon in player.GetComponent<LoadoutManager>().GetWeaponAbilities())
            player.GetComponentInChildren<AmmoManager>().SetAmmo(weapon.WeaponRef, 0);
    }
}
