using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : MonoBehaviour
{
    [SerializeField]
    bool NeedsToDie = false;

    void Awake()
    {
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += GetComponent<GameTrigger>().Activate;

        if (NeedsToDie)
        {
            GetComponent<Health>().OnDie += GetComponent<GameTrigger>().Destroy;
            if (GetComponent<UniqueID>())
                GetComponent<Health>().OnDie += GetComponent<UniqueID>().RegisterID;
        }
        else
        {
            GetComponent<Health>().OnDamage += (damage) => GetComponent<GameTrigger>().Activate();
            if (GetComponent<UniqueID>())
                GetComponent<Health>().OnDamage += (damage) => GetComponent<UniqueID>().RegisterID();
        }
    }
}
