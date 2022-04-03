using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : Health
{
    [SerializeField]
    int maxHealth = 5;

    protected override void Start()
    {
        MaxHealth.value = maxHealth;
        base.Start();

    }
}
