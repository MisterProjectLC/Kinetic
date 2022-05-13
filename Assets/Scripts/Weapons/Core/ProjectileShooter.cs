using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    GameObject Projectile;
    LayersConfig HitLayers;
    Transform Mouth;

    [HideInInspector]
    public List<GameObject> ActiveProjectiles;

    [SerializeField]
    float ExtraAutohitTime = 0f;

    Attack attack;

    void Start()
    {
        attack = GetComponent<Attack>();
    }

    public void Setup(GameObject Projectile, LayersConfig HitLayers, Transform Mouth)
    {
        this.Projectile = Projectile;
        this.HitLayers = HitLayers;
        this.Mouth = Mouth;
    }

    public GameObject ShootProjectile(Vector3 direction)
    {
        // Projectile attack
        GameObject instance = ObjectManager.OM.SpawnObjectFromPool(Projectile.GetComponent<Poolable>().Type, Projectile);
        instance.transform.position = Mouth.position;
        instance.GetComponent<Projectile>().Setup(direction, HitLayers.layers, GetComponentInParent<Actor>().gameObject, ExtraAutohitTime);

        if (attack)
        {
            Attack[] instanceAttacks = instance.GetComponents<Attack>();
            foreach (Attack instanceAttack in instanceAttacks)
            {
                if (instanceAttack)
                {
                    instanceAttack.Agressor = attack.Agressor;
                    attack.SetupClone(instanceAttack);
                }
            }

            if (instanceAttacks[0])
                if (!instance.GetComponent<Poolable>() || !instance.GetComponent<Poolable>().AlreadyInitialized)
                    instance.GetComponent<Projectile>().OnDestroy += RemoveProjectileFromList;
        }

        ActiveProjectiles.Add(instance);
        return instance;
    }

    private void RemoveProjectileFromList(Projectile proj)
    {
        ActiveProjectiles.Remove(proj.gameObject);
    }
}
