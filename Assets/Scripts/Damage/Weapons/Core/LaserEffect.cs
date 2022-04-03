using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HitscanWeapon))]
public class LaserEffect : MonoBehaviour
{
    [SerializeField]
    Material laserMaterial;
    Color laserColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField]
    float MaxDistance = 400f;

    [Header("References")]
    [SerializeField]
    GameObject LaserObject;
    [SerializeField]
    LayerMask layersConfig;
    LineRenderer Laser;
    HitscanWeapon WeaponRef;

    [SerializeField]
    [Tooltip("Set to negative to disable")]
    float DefaultAlpha = -1f;
    [SerializeField]
    float AlphaDecayRate = 1f;
    [HideInInspector]
    public float Alpha = 0f;

    private void Start()
    {
        GameObject newInstance = Instantiate(LaserObject);
        Laser = newInstance.GetComponent<LineRenderer>();
        Laser.material = laserMaterial;
        WeaponRef = GetComponent<HitscanWeapon>();
        WeaponRef.SubscribeToFire(OnFire);
    }

    void OnDisable()
    {
        Alpha = 0.01f;
        UpdateAlpha();
    }

    private void Update()
    {
        UpdateAlpha();
    }


    void UpdateAlpha()
    {
        if (Alpha < 0 || Alpha > 1f)
        {
            Alpha = 0f;
            return;
        }
        
        Alpha -= Time.deltaTime* AlphaDecayRate;
        laserColor.a = Alpha;
        if (Laser)
        {
            Laser.startColor = laserColor;
            Laser.endColor = laserColor;
        }
    }


    void OnFire(Weapon weapon)
    {
        Physics.Raycast(WeaponRef.Mouth.position, WeaponRef.Mouth.forward, out RaycastHit hit, MaxDistance, layersConfig);
        if (DefaultAlpha >= 0f)
            Alpha = DefaultAlpha;
        if (!hit.collider)
        {
            WeaponRef.MaxDistance = MaxDistance;
            Laser.SetPosition(0, WeaponRef.Mouth.position + (WeaponRef.Mouth.forward + WeaponRef.Mouth.right)*0.5f);
            Laser.SetPosition(1, WeaponRef.Mouth.position + WeaponRef.Mouth.forward * 400f);
        }
        else
        {
            WeaponRef.MaxDistance = hit.distance;
            Laser.SetPosition(0, WeaponRef.Mouth.position + (WeaponRef.Mouth.forward + WeaponRef.Mouth.right) * 0.5f);
            Laser.SetPosition(1, hit.point);
        }
    }
}
