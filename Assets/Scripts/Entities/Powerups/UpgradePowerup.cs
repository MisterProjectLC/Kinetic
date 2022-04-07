using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePowerup : Powerup
{
    [SerializeField]
    LevelUpSystem.Type type = LevelUpSystem.Type.Ability;

    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += () => { 
                Debug.Log("Destroyed: " + GetComponent<UniqueID>().ID); 
                Destroy(gameObject); 
            };

    }

    protected override void ActivatePowerup(GameObject player)
    {
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().RegisterID();

        Pause.Ps.TogglePause(0);
        LevelUpSystem.LUS.LevelUp(type);
    }
}
