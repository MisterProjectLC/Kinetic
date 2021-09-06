using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : BarUI
{
    Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = ActorsManager.Player.GetComponent<Health>();
        playerHealth.OnDamage += UpdateHealth;
        playerHealth.OnHeal += UpdateHealth;
    }

    void UpdateHealth()
    {
        UpdateBar(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }
}
