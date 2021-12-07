using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : WeaponAbility
{
    [Header("Attributes")]
    [SerializeField]
    float MinimumDamage = 1;
    [SerializeField]
    float MaximumDamage = 5;
    [SerializeField]
    float MinimumForce = 2;
    [SerializeField]
    float MaximumForce = 30;
    [SerializeField]
    float MaxCharge = 3f;

    [Header("Sounds")]
    [SerializeField]
    AudioClip ChargingSFX;

    [Header("References")]
    [SerializeField]
    LayerMask layersConfig;

    LaserEffect LaserEffect;
    [SerializeField]
    Color laserColor;

    bool charging = false;
    float charge = 0;
    float alpha = 0f;
    int killsSince = 0;

    Attack attack;

    new void Awake()
    {
        LaserEffect = GetComponent<LaserEffect>();
        attack = GetComponent<Attack>();
        attack.OnKill += (Attack a, GameObject g, bool b) => OnKill();
        base.Awake();
    }

    private void Start()
    {
        ReleaseAbility = true;
    }

    void OnDisable()
    {
        alpha = 0.01f;
    }

    void OnKill()
    {
        killsSince++;
        if (killsSince > 1)
            ResetCooldown();
    }


    public override void Execute(Input input)
    {
        if (!charging && charge == 0)
            PlaySound(ChargingSFX);

        charging = (input == Input.ButtonDown);
        if (charging)
        {
            if (charge < MaxCharge)
                charge += 10 * Time.deltaTime;
            else
                charge = MaxCharge;

            ResetCooldown();
        }
        else
        {
            float percentage = charge / MaxCharge;
            LaserEffect.Alpha = percentage;
            charge = 0f;
            BackwardsForce = Mathf.CeilToInt(Mathf.Lerp(MinimumForce, MaximumForce, percentage));
            attack.Damage = Mathf.CeilToInt(Mathf.Lerp(MinimumDamage, MaximumDamage, percentage));

            killsSince = 0;
            base.Execute(input);
        }
    }
}