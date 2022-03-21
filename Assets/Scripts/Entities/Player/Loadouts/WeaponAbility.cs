using UnityEngine;

public class WeaponAbility : Ability
{
    [Header("References")]
    public Weapon WeaponRef;

    protected PlayerCharacterController player;
    protected float BackwardsForce = 10f;


    public void Awake()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        HoldAbility = WeaponRef.Automatic;
        Cooldown = WeaponRef.FireCooldown;
        BackwardsForce = WeaponRef.BackwardsForce;
    }

    public override void Execute(Input input)
    {
        player.MoveVelocity -= BackwardsForce * player.PlayerCamera.transform.forward;
        if (GetComponent<Animator>())
            GetComponent<Animator>().SetTrigger("Shoot");
        WeaponRef.Trigger();
    }

    new public void ResetCooldown()
    {
        WeaponRef.ResetClock();
        ((Ability)this).ResetCooldown();
    }
}
