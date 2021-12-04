using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField]
    CrystalShield GeneratedShield;

    // Start is called before the first frame update
    void Start()
    {
        GeneratedShield.RegisterGenerator();
        GetComponent<Health>().OnDie += GeneratedShield.DisableGenerator;
    }
}
