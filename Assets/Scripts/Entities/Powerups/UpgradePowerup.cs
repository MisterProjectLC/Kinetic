using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePowerup : Powerup
{
    // Start is called before the first frame update
    void Awake()
    {
        OnPowerup += Upgrade;

        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += () => { Debug.Log("Destroyed: " + GetComponent<UniqueID>().ID); 
                Destroy(gameObject); };

    }

    void Upgrade(GameObject player)
    {
        if (GetComponent<UniqueID>())
        {
            Debug.Log("UniqueID: " + GetComponent<UniqueID>().ID);
            GetComponent<UniqueID>().RegisterID();
        }

        Pause.Ps.TogglePause(0);
        LevelUpSystem.LUS.LevelUp();
    }
}
