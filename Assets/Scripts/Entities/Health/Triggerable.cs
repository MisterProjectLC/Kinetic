using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : MonoBehaviour
{
    [SerializeField]
    bool NeedsToDie = false;

    void Start()
    {
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += GetComponent<GameTrigger>().Activate;

        if (NeedsToDie)
            GetComponent<Health>().OnDie += GetComponent<GameTrigger>().Destroy;
        else
            GetComponent<Health>().OnDamage += (damage) => GetComponent<GameTrigger>().Activate();
    }
}
