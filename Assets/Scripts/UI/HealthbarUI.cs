using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    Health playerHealth;

    [SerializeField]
    RectTransform healthbar;
    Vector2 healthbarSize;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = ActorsManager.Player.GetComponent<Health>();
        playerHealth.OnDamage += UpdateHealth;
        playerHealth.OnHeal += UpdateHealth;

        healthbarSize = healthbar.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealth()
    {
        healthbar.sizeDelta = healthbarSize * new Vector2((float)playerHealth.CurrentHealth / (float)playerHealth.MaxHealth, 1f);
    }
}
