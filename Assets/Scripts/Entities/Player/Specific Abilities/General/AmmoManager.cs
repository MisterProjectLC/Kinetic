using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmmoManager : MonoBehaviour
{
    List<WeaponAbility> weapons = new List<WeaponAbility>();
    List<GameObject> consumedPowerups = new List<GameObject>();

    Dictionary<StyleCrate.EnemyType, int> AmmoPerType = new Dictionary<StyleCrate.EnemyType, int> {
        {StyleCrate.EnemyType.Minion, 1}, {StyleCrate.EnemyType.BigMinion, 3}, {StyleCrate.EnemyType.Knight, 10},
        {StyleCrate.EnemyType.Boss, 10}
    };

    UnityAction OnOutOfAmmo;
    public void SubscribeToOutOfAmmo(UnityAction subscribee) { OnOutOfAmmo += subscribee; }

    UnityAction<Weapon> OnAmmoUpdate;
    public void SubscribeToAmmoUpdate(UnityAction<Weapon> subscribee) { OnAmmoUpdate += subscribee; }

    UnityAction<bool> OnInfiniteAmmo;
    public void SubscribeToInfiniteAmmo(UnityAction<bool> subscribee) { OnInfiniteAmmo += subscribee; }

    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacterController player = GetComponentInParent<PlayerCharacterController>();

        player.GetComponentInChildren<StyleMeter>().SubscribeToCritical(OnStyleCritical);

        foreach (WeaponAbility weapon in player.GetComponentsInChildren<WeaponAbility>())
        {
            weapons.Add(weapon);
            weapon.WeaponRef.SubscribeToFire(IndividualOnFire);
        }

        foreach (Attack attack in player.GetComponentsInChildren<Attack>())
            attack.OnKill += (Attack a, GameObject g, bool b) => ReplenishOnKill(g);
    }

    void OnStyleCritical(bool critical)
    {
        OnInfiniteAmmo?.Invoke(critical);
        foreach (WeaponAbility weapon in weapons)
            weapon.WeaponRef.InfiniteAmmo = critical;
    }

    void IndividualOnFire(Weapon firedWeapon)
    {
        OnAmmoUpdate?.Invoke(firedWeapon);
        if (firedWeapon.Ammo == 0)
        {
            foreach (WeaponAbility weapon in weapons)
                if (weapon.Assigned && weapon.WeaponRef.Ammo > 0)
                    return;

            OutOfAmmo();
        }
    }

    void OutOfAmmo()
    {
        OnOutOfAmmo?.Invoke();
        foreach (GameObject powerup in consumedPowerups)
            if (powerup)
                powerup.SetActive(true);
        consumedPowerups.Clear();
    }

    void ReplenishOnKill(GameObject victim)
    {
        StyleCrate styleCrate = victim.GetComponent<StyleCrate>();

        if (!styleCrate)
            return;

        foreach (WeaponAbility weapon in weapons)
        {
            weapon.WeaponRef.ReplenishAmmo(AmmoPerType[styleCrate.enemyType]);
            OnAmmoUpdate?.Invoke(weapon.WeaponRef);
        }
    }


    public void ReplenishOnPowerup(GameObject consumedPowerup)
    {
        consumedPowerups.Add(consumedPowerup);
        foreach (WeaponAbility weapon in weapons)
        {
            weapon.WeaponRef.ReplenishAmmo(100);
            OnAmmoUpdate?.Invoke(weapon.WeaponRef);
        }
    }

}