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
            GetComponent<UniqueID>().OnObjectRegistered += ActivateTrigger;

        if (NeedsToDie)
            GetComponent<Health>().OnDie += GetComponent<GameTrigger>().OnTriggerActivate;
        else
            GetComponent<Health>().OnDamage += ActivateTrigger;
    }

    void ActivateTrigger()
    {
        GetComponent<GameTrigger>().OnTriggerActivate();
    }

    void ActivateTrigger(int damage)
    {
        GetComponent<GameTrigger>().OnTriggerActivate();
    }
}
