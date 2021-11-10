using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePowerup : Powerup
{
    // Start is called before the first frame update
    void Start()
    {
        OnPowerup += Upgrade;
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += () => Destroy(gameObject);

    }

    void Upgrade(GameObject player)
    {
        Pause.Ps.TogglePause(0);
        LevelUpSystem.LUS.LevelUp();
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().RegisterID();
    }
}
