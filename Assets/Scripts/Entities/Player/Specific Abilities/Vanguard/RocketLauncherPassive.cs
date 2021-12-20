using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherPassive : MonoBehaviour
{
    ProjectileWeapon weapon;
    Ability ability;

    [SerializeField]
    float quickCooldown = 0.2f;

    [SerializeField]
    float slowCooldown = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<ProjectileWeapon>();
        ability = GetComponent<Ability>();
        GetComponentInParent<StyleMeter>().OnCritical += OnCritical;
    }

    void OnCritical(bool critical)
    {
        float newCooldown = critical ? quickCooldown : slowCooldown;
        weapon.FireCooldown = newCooldown;
        ability.Cooldown = newCooldown;
    }
}
