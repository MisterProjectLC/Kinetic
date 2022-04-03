using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyWeaponsManager : IEnemyWeaponsManager
{
    public bool OnlyShootIfPlayerInView = true;

    Enemy enemy;
    protected RaycastHit[] hits;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.SubscribeToUpdate(ActivateWeapons);
    }

    override public void ActivateWeapons()
    {
        if (OnlyShootIfPlayerInView && !enemy.IsPlayerInView())
            return;

        foreach (Weapon weapon in weapons)
            if (weapon)
                weapon.Trigger();
    }
}
