using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerup : Powerup
{
    protected override void ActivatePowerup(GameObject player)
    {
        player.GetComponentInChildren<AmmoManager>().ReplenishOnPowerup(gameObject);
    }


}
