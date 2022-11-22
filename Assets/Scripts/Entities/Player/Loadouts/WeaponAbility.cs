using UnityEngine;

public class WeaponAbility : Ability
{
    [Header("References")]
    public Weapon WeaponRef;

    protected PlayerCharacterController player;
    protected float BackwardsForce = 10f;


    protected new void Awake()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        HoldAbility = WeaponRef.Automatic;
        Cooldown = WeaponRef.FireCooldown;
        BackwardsForce = WeaponRef.BackwardsForce;
        base.Awake();
    }

    public override void Execute(Input input)
    {
        player.ReceiveForce(-BackwardsForce * player.GetPlayerCamera().transform.forward);
        if (GetComponent<Animator>())
            GetComponent<Animator>().SetTrigger("Shoot");

        WeaponRef.Trigger();
    }

    new public void ResetCooldown()
    {
        WeaponRef.ResetClock();
        base.ResetCooldown();
    }
}
