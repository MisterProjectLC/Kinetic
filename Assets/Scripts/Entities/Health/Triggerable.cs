using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : MonoBehaviour
{
    [SerializeField]
    bool NeedsToDie = false;

    // Start is called before the first frame update
    void Start()
    {
        if (NeedsToDie)
            GetComponent<Health>().OnDie += GetComponent<GameTrigger>().OnTriggerActivate;
        else
            GetComponent<Health>().OnDamage += GetComponent<GameTrigger>().OnTriggerActivate;
    }
}
