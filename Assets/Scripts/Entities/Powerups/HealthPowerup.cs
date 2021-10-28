using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : Powerup
{

    // Start is called before the first frame update
    void Start()
    {
        OnPowerup += Heal;
    }

    void Heal(GameObject player)
    {
        if (player.GetComponent<Health>())
            player.GetComponent<Health>().Heal(player.GetComponent<Health>().MaxHealth);
    }
}
