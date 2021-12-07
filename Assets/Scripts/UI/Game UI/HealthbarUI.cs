using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : BarUI
{
    [SerializeField]
    [Tooltip("If null, takes the player health")]
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        if (health == null)
            health = ActorsManager.Player.GetComponent<Health>();
        health.OnDamage += UpdateHealth;
        health.OnHeal += UpdateHealth;
    }

    void UpdateHealth(int change)
    {
        UpdateBar(health.CurrentHealth, health.MaxHealth);
    }
}
