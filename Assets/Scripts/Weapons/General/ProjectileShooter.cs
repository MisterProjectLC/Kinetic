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
        instance.GetComponent<Projectile>().Setup(direction, HitLayers.layers, GetComponentInParent<Actor>().gameObject);

        if (instance.GetComponent<Attack>() && GetComponent<Attack>())
        {
            instance.GetComponent<Attack>().Agressor = GetComponent<Attack>().Agressor;
            if (!instance.GetComponent<Poolable>() || !instance.GetComponent<Poolable>().AlreadyInitialized)
            {
                GetComponent<Attack>().SetupClone(instance.GetComponent<Attack>());
                instance.GetComponent<Projectile>().OnDestroy += RemoveProjectileFromList;
            }
        }

        ActiveProjectiles.Add(instance);
        return instance;
    }

    private void RemoveProjectileFromList(Projectile proj)
    {
        ActiveProjectiles.Remove(proj.gameObject);
    }
}
