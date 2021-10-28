using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePowerup : Powerup
{
    // Start is called before the first frame update
    void Start()
    {
        OnPowerup += Upgrade;
    }

    void Upgrade(GameObject player)
    {
        Pause.Ps.TogglePause();
        LevelUpSystem.LUS.LevelUp();
    }
}
