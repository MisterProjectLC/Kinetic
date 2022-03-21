using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerup : Powerup
{
    // Start is called before the first frame update
    protected override void Setup()
    {
        OnPowerup += ReplenishAmmo;
    }

    void ReplenishAmmo(GameObject player)
    {
        player.GetComponentInChildren<AmmoManager>().ReplenishOnPowerup(gameObject);
    }


}
