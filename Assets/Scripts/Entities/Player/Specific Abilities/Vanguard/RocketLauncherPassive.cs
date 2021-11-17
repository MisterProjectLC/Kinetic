using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherPassive : MonoBehaviour
{
    ProjectileWeapon weapon;
    Ability ability;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<ProjectileWeapon>();
        ability = GetComponent<Ability>();
        GetComponentInParent<StyleMeter>().OnCritical += OnCritical;
    }

    void OnCritical(bool critical)
    {
        float newCooldown = critical ? 0.2f : 1f;
        weapon.FireCooldown = newCooldown;
        ability.Cooldown = newCooldown;
        Debug.Log("Critical: " + newCooldown);
    }
}
