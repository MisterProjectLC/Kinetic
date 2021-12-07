using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    Weapon WeaponRef;

    [SerializeField]
    [Tooltip("Set to negative to disable")]
    float DefaultAlpha = -1f;
    [HideInInspector]
    public float Alpha = 0f;

    private void Start()
    {
        GameObject newInstance = Instantiate(LaserObject);
        Laser = newInstance.GetComponent<LineRenderer>();
        Laser.material = laserMaterial;
        WeaponRef = GetComponent<Weapon>();
        WeaponRef.OnFire += OnFire;
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
        if (Alpha > 0)
        {
            Alpha -= Time.deltaTime;
            laserColor.a = Alpha;
            if (Laser)
            {
                Laser.startColor = laserColor;
                Laser.endColor = laserColor;
            }
        }
    }


    void OnFire()
    {
        Physics.Raycast(WeaponRef.Mouth.position, WeaponRef.Mouth.forward, out RaycastHit hit, MaxDistance, layersConfig);
        if (DefaultAlpha >= 0f)
            Alpha = DefaultAlpha;
        if (!hit.collider)
        {
            ((HitscanWeapon)WeaponRef).MaxDistance = MaxDistance;
            Laser.SetPosition(0, WeaponRef.Mouth.position + (WeaponRef.Mouth.forward + WeaponRef.Mouth.right)*0.5f);
            Laser.SetPosition(1, WeaponRef.Mouth.position + WeaponRef.Mouth.forward * 400f);
        }
        else
        {
            ((HitscanWeapon)WeaponRef).MaxDistance = hit.distance;
            Laser.SetPosition(0, WeaponRef.Mouth.position + (WeaponRef.Mouth.forward + WeaponRef.Mouth.right) * 0.5f);
            Laser.SetPosition(1, hit.point);
        }
    }
}
