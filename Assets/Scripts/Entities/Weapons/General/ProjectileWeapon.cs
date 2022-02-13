using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileShooter))]
public class ProjectileWeapon : Weapon
{
    [SerializeField]
    private GameObject Projectile;
    [HideInInspector]
    public List<GameObject> ActiveProjectiles { get { return shooter.ActiveProjectiles; } }

    ProjectileShooter shooter;

    private void Awake()
    {
        shooter = GetComponent<ProjectileShooter>();
        shooter.Setup(Projectile, HitLayers, Mouth);
    }

    public override void Shoot(Vector3 direction)
    {
        shooter.ShootProjectile(direction);
    }
}
