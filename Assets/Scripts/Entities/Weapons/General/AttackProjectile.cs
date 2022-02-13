using System.Collections.Generic;
using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    public ObjectManager.PoolableType ImpactType;
    public GameObject ImpactObject;
    public int PenetrateCount = 0;
    List<Health> previousHits = new List<Health>();
    int penetrateCount = 0;
    

    private void Start()
    {
        GetComponent<Projectile>().OnHit += OnHit;
        penetrateCount = PenetrateCount;
        previousHits = new List<Health>(penetrateCount);
    }

    private void OnEnable()
    {
        penetrateCount = PenetrateCount;
        previousHits.Clear();
    }

    private void OnHit(Collider collider)
    {
        if (!collider.gameObject.GetComponent<Damageable>() || 
            previousHits.Contains(collider.gameObject.GetComponent<Damageable>().GetHealth()))
            return;

        if (GetComponent<Attack>())
            GetComponent<Attack>().AttackTarget(collider.gameObject);

        SpawnImpactObject();

        if (penetrateCount > 0)
        {
            penetrateCount--;
            previousHits.Add(collider.gameObject.GetComponent<Damageable>().GetHealth());
        }
        else
            GetComponent<Projectile>().Destroy();
    }

    public void Detonate()
    {
        SpawnImpactObject();
        GetComponent<Projectile>().Destroy();
    }

    private void SpawnImpactObject()
    {
        if (!ImpactObject)
            return;

        GameObject newObject = ObjectManager.OM.SpawnObjectFromPool(ImpactObject.GetComponent<Poolable>().Type, ImpactObject);
        newObject.transform.position = transform.position;
        if (newObject.GetComponent<Attack>() && GetComponent<Attack>())
        {
            newObject.GetComponent<Attack>().Agressor = GetComponent<Attack>().Agressor;
            if (!newObject.GetComponent<Poolable>() || !newObject.GetComponent<Poolable>().AlreadyInitialized)
                GetComponent<Attack>().SetupClone(newObject.GetComponent<Attack>());
        }
    }
}
