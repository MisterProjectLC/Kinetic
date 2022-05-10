using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Pickup
{
    [SerializeField]
    int abilityIndex;
    [SerializeField]
    int loadout;
    [SerializeField]
    int slot;

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

        LoadoutManager loadoutManager = player.GetComponent<LoadoutManager>();
        loadoutManager.SetAbility((Ability)loadoutManager.GetInitialOptions()[abilityIndex].option, loadout, slot);
    }
}
