using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayer : Health
{
    [SerializeField]
    int playerMaxHealth = 5;

    protected override void Start()
    {
        MaxHealth = playerMaxHealth;
        base.Start();

    }
}
