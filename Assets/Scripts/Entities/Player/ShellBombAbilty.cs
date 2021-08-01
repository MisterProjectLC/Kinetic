using System.Collections;
using UnityEngine;

public class ShellBombAbilty : Ability
{
    [Header("General")]
    public GameObject ShellBomb;
    public LayerMask HitLayers;

    [Header("Attributes")]
    public int ShellCount = 3;
    public float ShellCooldown = 0.1f;

    PlayerCharacterController player;

    public void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
    }

    public override void Execute()
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
        GameObject newInstance = ObjectManager.OM.SpawnObjectFromPool(ObjectManager.PoolableType.ShellBomb, ShellBomb);
        newInstance.transform.position = player.PlayerCamera.transform.position;
        newInstance.GetComponent<Projectile>().Setup(player.PlayerCamera.transform.forward, HitLayers);
    }
}