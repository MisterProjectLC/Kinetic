using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    public ObjectManager.PoolableType ImpactType;
    public GameObject ImpactObject;
    public int PenetrateCount = 0;

    private void Start()
    {
        GetComponent<Projectile>().OnHit += OnHit;
    }

    private void OnHit(Collider collider)
    {
        if (GetComponent<Attack>())
            GetComponent<Attack>().AttackTarget(collider.gameObject);
        GameObject newObject = ObjectManager.OM.SpawnObjectFromPool(ImpactType, ImpactObject).gameObject;
        newObject.transform.position = transform.position;
        if (PenetrateCount > 0)
            PenetrateCount--;
        else
            ObjectManager.OM.EraseObject(GetComponent<Poolable>());
    }
}
