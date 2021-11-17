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

        SpawnImpactObject();

        if (PenetrateCount > 0)
            PenetrateCount--;
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
        GameObject newObject = ObjectManager.OM.SpawnObjectFromPool(ImpactType, ImpactObject);
        newObject.transform.position = transform.position;
        if (newObject.GetComponent<Attack>() && GetComponent<Attack>())
        {
            newObject.GetComponent<Attack>().Agressor = GetComponent<Attack>().Agressor;
            if (!newObject.GetComponent<Poolable>() || !newObject.GetComponent<Poolable>().AlreadyInitialized)
                GetComponent<Attack>().SetupClone(newObject.GetComponent<Attack>());
        }
    }
}
