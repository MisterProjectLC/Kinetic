using UnityEngine;

public class WeaponAbility : Ability
{
    [Header("References")]
    public Weapon WeaponRef;

    protected PlayerCharacterController player;
    Camera playerCamera;
    protected float BackwardsForce = 10f;


    protected void Awake()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        HoldAbility = WeaponRef.Automatic;
        Cooldown = WeaponRef.FireCooldown;
        BackwardsForce = WeaponRef.BackwardsForce;
        base.Awake();
    }

    private void OnEnable()
    {
        playerCamera = player.GetPlayerCamera();
        base.OnEnable();
    }

    public override void Execute(Input input)
    {
        player.ReceiveForce(-BackwardsForce * playerCamera.transform.forward);
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
