using System.Collections;
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
    [SerializeField]
    AudioClip CollisionSFX;

    bool charging = false;
    bool charged = false;
    float charge = 0;

    bool critical = true;

    PlayerCharacterController player;
    Attack attack;

    // Start is called before the first frame update
    public void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        player.OnCollision += Colliding;
        OnUpdate += StopCharging;
        attack = GetComponent<Attack>();
        GetComponentInParent<StyleMeter>().OnCritical += OnCritical;

        foreach (Attack attack in GetComponentInParent<PlayerCharacterController>().GetComponentsInChildren<Attack>())
            attack.OnKill += (Attack a, GameObject g, bool b) => OnKill();
    }

    void OnCritical(bool critical)
    {
        this.critical = critical;
    }

    void OnKill()
    {
        if (critical)
            ResetCooldown();
    }


    void StopCharging()
    {
        if (charged && player.MoveVelocity.magnitude < MinimumForce / 2)
            StartCoroutine("ChargeFalse");
    }

    IEnumerator ChargeFalse()
    {
        yield return new WaitForSeconds(0.5f);
        charged = false;
    }
    

    void Colliding(ControllerColliderHit hit)
    {
        if (charged)
        {
            attack.AttackTarget(hit.gameObject);
            PlaySound(CollisionSFX);
            charged = false;
        }
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
            player.MoveVelocity = player.MoveVelocity * (1f - charge / MaxCharge);
            if (charge < MaxCharge)
                charge += 10*Time.deltaTime;
            else
                charge = MaxCharge;

            ResetCooldown();
        } else
        {
            player.SetSlowdown(1f, "charge");
            player.SetSlowdown(1f, "charge", false);

            if (player.IsGrounded)
                player.Translate(Vector3.up*1.15f);
            player.ApplyForce(Mathf.Lerp(MinimumForce, MaximumForce, charge / MaxCharge) *
                player.PlayerCamera.transform.TransformVector(Vector3.forward));
            charge = 0;

            PlaySound(ChargeSFX);
            charged = true;
        }
    }
}
