using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Ability
{
    [Header("Attributes")]
    [SerializeField]
    float MinimumForce = 1;
    [SerializeField]
    float MaximumForce = 10;
    [SerializeField]
    float MaxCharge = 3f;
    [SerializeField]
    float MaxGroundSlowdown = 0.25f;
    [SerializeField]
    float MaxAirSlowdown = 0.6f;

    [Header("Sounds")]
    [SerializeField]
    AudioClip ChargingSFX;
    [SerializeField]
    AudioClip ChargeSFX;

    bool charging = false;
    bool charged = false;
    float charge = 0;

    PlayerCharacterController player;
    Attack attack;

    // Start is called before the first frame update
    public void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        player.OnCollision += Colliding;
        OnUpdate += StopCharging;
        attack = GetComponent<Attack>();
    }

    void StopCharging()
    {
        if (charged && player.MoveVelocity.magnitude < (MinimumForce+MaximumForce)/2)
            charged = false;
    }
    

    void Colliding(ControllerColliderHit hit)
    {
        if (charged)
            attack.AttackTarget(hit.gameObject);
    }


    public override void Execute(Input input)
    {
        if (!charging && charge == 0)
            PlaySound(ChargingSFX);

        charging = (input == Input.ButtonDown);
        if (charging)
        {
            player.SetSlowdown(Mathf.Lerp(1f, MaxGroundSlowdown, charge/MaxCharge), "charge");
            player.SetSlowdown(Mathf.Lerp(1f, MaxAirSlowdown, charge/MaxCharge), "charge", false);
            if (charge < MaxCharge)
                charge += 10*Time.deltaTime;
            else
                charge = MaxCharge;
            Debug.Log(charge);
            ResetCooldown();
        } else
        {
            player.SetSlowdown(1f, "charge");
            player.SetSlowdown(1f, "charge", false);

            if (player.IsGrounded)
                player.Translate(Vector3.up*1.15f);
            player.ApplyForce(Mathf.Lerp(MinimumForce, MaximumForce, charge / MaxCharge) *
                Vector3.ProjectOnPlane(player.PlayerCamera.transform.TransformVector(Vector3.forward), Vector3.up));
            charge = 0;

            PlaySound(ChargeSFX);
            charged = true;
        }
    }
}
