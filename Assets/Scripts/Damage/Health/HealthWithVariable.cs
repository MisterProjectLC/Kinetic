using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthWithVariable : Health
{
    [SerializeField]
    int DefaultMaxHealth = 5;

    protected override void Start()
    {
        MaxHealth = DefaultMaxHealth;
        base.Start();

    }
}
