using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : Powerup
{

    // Start is called before the first frame update
    protected override void Setup()
    {
        OnPowerup += Heal;
    }

    void Heal(GameObject player)
    {
        if (player.GetComponent<Health>())
            player.GetComponent<Health>().Heal(player.GetComponent<Health>().MaxHealth);
    }

    protected override bool ValidPowerup(PlayerCharacterController player)
    {
        Health health = player.GetComponent<Health>();
        return health.CurrentHealth < health.MaxHealth;
    }
}
