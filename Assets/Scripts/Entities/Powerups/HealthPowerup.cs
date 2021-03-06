using UnityEngine;

public class HealthPowerup : Powerup
{
    protected override void ActivatePowerup(GameObject player)
    {
        if (player.GetComponent<Health>())
            player.GetComponent<Health>().Heal(player.GetComponent<Health>().MaxHealth);
    }

    protected override bool ValidPowerup(PlayerCharacterController player)
    {
        Health health = player.GetComponent<Health>();
        return health.CurrentHealth < health.MaxHealth;
    }
}
