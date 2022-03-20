using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmmoManager : MonoBehaviour
{
    List<Weapon> weapons = new List<Weapon>();
    List<GameObject> consumedPowerups = new List<GameObject>();

    Dictionary<StyleCrate.EnemyType, int> AmmoPerType = new Dictionary<StyleCrate.EnemyType, int> {
        {StyleCrate.EnemyType.Minion, 1}, {StyleCrate.EnemyType.BigMinion, 3}, {StyleCrate.EnemyType.Knight, 10},
        {StyleCrate.EnemyType.Boss, 10}
    };

    UnityAction OnOutOfAmmo;
    public void SubscribeToOutOfAmmo(UnityAction subscribee) { OnOutOfAmmo += subscribee; }
    UnityAction<bool> OnInfiniteAmmo;
    public void SubscribeToInfiniteAmmo(UnityAction<bool> subscribee) { OnInfiniteAmmo += subscribee; }

    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacterController player = GetComponentInParent<PlayerCharacterController>();

        player.GetComponentInChildren<StyleMeter>().SubscribeToCritical(OnStyleCritical);

        foreach (WeaponAbility weapon in player.GetComponentsInChildren<WeaponAbility>())
        {
            weapons.Add(weapon.WeaponRef);
            weapon.WeaponRef.SubscribeToOutOfAmmo(IndividualOutOfAmmo);
        }

        foreach (Attack attack in player.GetComponentsInChildren<Attack>())
        {
            attack.OnKill += (Attack a, GameObject g, bool b) => ReplenishOnKill(g);
        }
    }

    void OnStyleCritical(bool critical)
    {
        OnInfiniteAmmo?.Invoke(critical);
        foreach (Weapon weapon in weapons)
            weapon.InfiniteAmmo = critical;
    }


    void IndividualOutOfAmmo()
    {

    }

    void ReplenishOnKill(GameObject victim)
    {
        StyleCrate styleCrate = victim.GetComponent<StyleCrate>();

        if (!styleCrate)
            return;

        foreach (Weapon weapon in weapons)
            weapon.ReplenishAmmo(AmmoPerType[styleCrate.enemyType]);
    }


    public void ReplenishOnPowerup(GameObject consumedPowerup)
    {
        consumedPowerups.Add(consumedPowerup);
        foreach (Weapon weapon in weapons)
            weapon.ReplenishAmmo(100);
    }

}