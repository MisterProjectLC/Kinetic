using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    [System.Serializable]
    public struct Loadout
    {
        public Ability[] abilities;
    }

    [Tooltip("List of currently equipped loadouts")]
    public Loadout[] Loadouts;

    private int currentLoadout = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public Ability[] GetCurrentLoadout()
    {
        return Loadouts[currentLoadout].abilities;
    }
}
