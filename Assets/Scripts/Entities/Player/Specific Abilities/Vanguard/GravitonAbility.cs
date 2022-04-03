using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitonAbility : Ability
{
    [Header("General")]
    public GameObject Graviton;
    public LayersConfig HitLayers;

    PlayerCharacterController player;
    ProjectileShooter shooter;

    void Awake()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        shooter = GetComponent<ProjectileShooter>();
        shooter.Setup(Graviton, HitLayers, player.GetPlayerCamera().transform);
    }

    public override void Execute(Input input)
    {
        if (shooter.ActiveProjectiles.Count <= 0)
        {
            shooter.ShootProjectile(player.GetPlayerCamera().transform.forward).GetComponent<Projectile>().OnDestroy += (Projectile p) => SetOffCooldown();
            ResetCooldown();
        } else
        {
            int control = 1000;
            while (shooter.ActiveProjectiles.Count > 0 && control > 0)
            {
                control--;
                shooter.ActiveProjectiles[0].GetComponent<AttackProjectile>().Detonate();
            }
        }
    }
}
