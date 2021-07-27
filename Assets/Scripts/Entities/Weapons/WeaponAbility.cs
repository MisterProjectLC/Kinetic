using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAbility : Ability
{
    [Header("References")]
    public Weapon WeaponRef;

    PlayerCharacterController player;
    private float BackwardsForce = 10f;


    public void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        DisplayName = WeaponRef.DisplayName;
        HoldAbility = WeaponRef.Automatic;
        Cooldown = WeaponRef.FireCooldown;
        BackwardsForce = WeaponRef.BackwardsForce;
    }


    public override void Execute()
    {
        player.MoveVelocity -= BackwardsForce * player.PlayerCamera.transform.forward;
        WeaponRef.Fire();
    }
}
