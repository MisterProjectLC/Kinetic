using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [HideInInspector]
    public static ObjectManager OM;
    [HideInInspector]
    public Stack<Poolable>[] Poolables;

    public int PoolableMaxCount = 200;

    public enum PoolableType
    {
        LaserSparks = 0,
        LaserProjectile,
        ShellBomb,
        ShellBombExplosion,
        Rocket,
        RocketLauncherExplosion,
        PlayerLaserProjectile,
        CentibombExplosion,
        EnemyRocket,
        RocketeerExplosion,
        GravitonProjectile,
        Graviton,
        SuspendedCrate,
        PaybackExplosion,
        ConveyorBeltDetail,
        KatanaSlash,
        Count
    }

    void Awake()
    {
        if (!OM)
            OM = this;
        else {
            Destroy(gameObject);
            return;
        }

        Poolables = new Stack<Poolable>[(int)PoolableType.Count];
        for (int i = 0; i < (int)PoolableType.Count; i++)
            //Poolables[i] = new Poolable[PoolableMaxCount];
            Poolables[i] = new Stack<Poolable>();
    }


    public GameObject SpawnObjectFromPool(PoolableType type, GameObject gameObject)
    {
        if (Poolables[(int)type].Count > 0)
        {
            GameObject newObject = Poolables[(int)type].Pop().gameObject;
            newObject.SetActive(true);
            return newObject;
        }
        else
        {
            return Instantiate(gameObject);
        }
    }


    public void EraseObject(Poolable instance)
    {
        instance.AlreadyInitialized = true;
        instance.gameObject.SetActive(false);
        Poolables[(int)instance.Type].Push(instance);
    }
}
