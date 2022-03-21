using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePowerup : Powerup
{
    [SerializeField]
    LevelUpSystem.Type type = LevelUpSystem.Type.Skill;

    // Start is called before the first frame update
    void Awake()
    {
        OnPowerup += Upgrade;

        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += () => { 
                Debug.Log("Destroyed: " + GetComponent<UniqueID>().ID); 
                Destroy(gameObject); 
            };

    }

    protected override void Setup() { }

    void Upgrade(GameObject player)
    {
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().RegisterID();

        Pause.Ps.TogglePause(0);
        LevelUpSystem.LUS.LevelUp(type);
    }
}
