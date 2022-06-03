using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [HideInInspector]
    static Dictionary<PoolableEnum, Stack<Poolable>> Poolables = new Dictionary<PoolableEnum, Stack<Poolable>>();

    static int POOLABLE_MAX_COUNT = 200;

    private void Awake()
    {
        ResetPool();
    }

    public static GameObject SpawnObjectFromPool(PoolableEnum type, GameObject gameObject)
    {
        if (!Poolables.ContainsKey(type))
            Poolables.Add(type, new Stack<Poolable>(POOLABLE_MAX_COUNT));


        if (Poolables[type].Count > 0)
        {
            GameObject newObject = Poolables[type].Pop().gameObject;
            newObject.SetActive(true);
            return newObject;
        }
        else
        {
            return Instantiate(gameObject);
        }
    }


    public static void EraseObject(Poolable instance)
    {
        instance.AlreadyInitialized = true;
        instance.gameObject.SetActive(false);

        if (!Poolables.ContainsKey(instance.Type))
            Poolables.Add(instance.Type, new Stack<Poolable>(POOLABLE_MAX_COUNT));

        Poolables[instance.Type].Push(instance);
    }

    public static void ResetPool()
    {
        Poolables = new Dictionary<PoolableEnum, Stack<Poolable>>();
    }
}
