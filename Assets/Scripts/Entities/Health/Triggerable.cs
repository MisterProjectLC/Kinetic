using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().OnDamage += GetComponent<GameTrigger>().OnTriggerActivate;
    }
}
