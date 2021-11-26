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
    [SerializeField]
    float MaxDistance = 400f;

    [Header("Sounds")]
    [SerializeField]
    AudioClip ChargingSFX;

    [Header("References")]
    [SerializeField]
    LayerMask layersConfig;

    [SerializeField]
    GameObject LaserObject;
    LineRenderer Laser;
    [SerializeField]
    Color laserColor;

    bool charging = false;
    float charge = 0;
    float alpha = 0f;
    int killsSince = 0;

    Attack attack;

    new void Awake()
    {
        GameObject newInstance = Instantiate(LaserObject);
        Laser = newInstance.GetComponent<LineRenderer>();

        attack = GetComponent<Attack>();
        attack.OnKill += (Attack a, GameObject g, bool b) => OnKill();
        base.Awake();
    }

    private void Start()
    {
        ReleaseAbility = true;
        OnUpdate += UpdateAlpha;
    }

    void OnDisable()
    {
        alpha = 0.01f;
        UpdateAlpha();
    }

    void OnKill()
    {
        killsSince++;
        if (killsSince > 1)
            ResetCooldown();
    }

    void UpdateAlpha()
    {
        if (alpha > 0)
        {
            alpha -= Time.deltaTime;
            laserColor.a = alpha;
            if (Laser)
            {
                Laser.startColor = laserColor;
                Laser.endColor = laserColor;
            }
        }
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
            charge = 0f;
            BackwardsForce = Mathf.CeilToInt(Mathf.Lerp(MinimumForce, MaximumForce, percentage));
            attack.Damage = Mathf.CeilToInt(Mathf.Lerp(MinimumDamage, MaximumDamage, percentage));

            Physics.Raycast(WeaponRef.Mouth.position, WeaponRef.Mouth.forward, out RaycastHit hit, MaxDistance, layersConfig);
            alpha = percentage;
            if (!hit.collider)
            {
                ((HitscanWeapon)WeaponRef).MaxDistance = MaxDistance;
                Laser.SetPosition(0, transform.position);
                Laser.SetPosition(1, transform.position + WeaponRef.Mouth.forward*400f);
            }
            else
            {
                ((HitscanWeapon)WeaponRef).MaxDistance = hit.distance;
                Laser.SetPosition(0, transform.position);
                Laser.SetPosition(1, hit.point);
            }

            killsSince = 0;
            base.Execute(input);
        }
    }
}