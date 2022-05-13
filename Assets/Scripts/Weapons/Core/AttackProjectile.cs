using System.Collections.Generic;
using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    public ObjectManager.PoolableType ImpactType;
    [SerializeField]
    GameObject ImpactObject;
    GameObject impactObject;

    public int PenetrateCount = 0;
    List<Health> previousHits = new List<Health>();
    int penetrateCount = 0;

    Attack attack;
    Projectile projectile;


    private void Start()
    {
        attack = GetComponent<Attack>();
        projectile = GetComponent<Projectile>();

        projectile.OnHit += OnHit;
        penetrateCount = PenetrateCount;
        previousHits = new List<Health>(penetrateCount);
    }

    private void OnEnable()
    {
        impactObject = ImpactObject;
        penetrateCount = PenetrateCount;
        previousHits.Clear();
    }

    private void OnHit(Collider collider)
    {
        if (collider.gameObject.GetComponent<Damageable>() && 
            previousHits.Contains(collider.gameObject.GetComponent<Damageable>().GetHealth()))
            return;

        if (attack)
            attack.AttackTarget(collider.gameObject);

        SpawnImpactObject();

        if (penetrateCount > 0 && collider.gameObject.GetComponent<Damageable>())
        {
            penetrateCount--;
            previousHits.Add(collider.gameObject.GetComponent<Damageable>().GetHealth());
        }
        else
        {
            projectile.Destroy();
        }
    }

    public GameObject Detonate()
    {
        GameObject newObject = SpawnImpactObject();
        projectile.Destroy();
        return newObject;
    }

    private GameObject SpawnImpactObject()
    {
        if (!impactObject)
            return null;

        GameObject instance = ObjectManager.OM.SpawnObjectFromPool(impactObject.GetComponent<Poolable>().Type, impactObject);
        instance.transform.position = transform.position;

        Attack[] instanceAttacks = instance.GetComponents<Attack>();
        foreach (Attack instanceAttack in instanceAttacks)
        {
            if (instanceAttack && attack)
            {
                instanceAttack.Agressor = attack.Agressor;
                attack.SetupClone(instanceAttack);
            }
        }

        return instance;
    }

    public void SetImpactObject(GameObject obj)
    {
        impactObject = obj;
    }
}
