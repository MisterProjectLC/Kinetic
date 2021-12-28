using System.Collections;
using UnityEngine;

public class ShellBombAbilty : Ability
{
    [Header("General")]
    public GameObject ShellBomb;
    public LayersConfig HitLayers;

    [Header("Attributes")]
    public int ShellCount = 3;
    public float ShellCooldown = 0.1f;

    PlayerCharacterController player;
    ProjectileShooter shooter;

    public void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        shooter = GetComponent<ProjectileShooter>();
        shooter.Setup(ShellBomb, HitLayers, player.PlayerCamera.transform);
    }

    public override void Execute(Input input)
    {
        StartCoroutine(RunShellBombs());
    }


    IEnumerator RunShellBombs()
    {
        for (int i = 0; i < ShellCount; i++)
        {
            ShootShellBombs();
            yield return new WaitForSeconds(ShellCooldown);
        }
    }

    void ShootShellBombs()
    {
        shooter.ShootProjectile(player.PlayerCamera.transform.forward);
    }
}