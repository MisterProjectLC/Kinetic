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

        // Spawn Impact Object
        GameObject newObject = ObjectManager.OM.SpawnObjectFromPool(ImpactType, ImpactObject).gameObject;
        newObject.transform.position = transform.position;
        if (newObject.GetComponent<Attack>() && GetComponent<Attack>() && 
            (!newObject.GetComponent<Poolable>() || !newObject.GetComponent<Poolable>().alreadyInitialized))
        {
            Attack theirAttack = newObject.GetComponent<Attack>();
            theirAttack.Agressor = GetComponent<Attack>().Agressor;
            theirAttack.OnAttack += GetComponent<Attack>().OnAttack;
            theirAttack.OnKill += GetComponent<Attack>().OnKill;
        }

        if (PenetrateCount > 0)
            PenetrateCount--;
        else
            GetComponent<Projectile>().Destroy();
    }

    public void Detonate()
    {
        GameObject newObject = ObjectManager.OM.SpawnObjectFromPool(ImpactType, ImpactObject).gameObject;
        newObject.transform.position = transform.position;

        GetComponent<Projectile>().Destroy();
    }
}
