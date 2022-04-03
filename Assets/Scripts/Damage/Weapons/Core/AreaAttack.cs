using System.Collections.Generic;
using UnityEngine;

public class AreaAttack
{
    public struct CollisionData
    {
        public Health health;
        public Collider closestCollider;

        public CollisionData(Health health, Collider collider)
        {
            this.health = health;
            this.closestCollider = collider;
        }
    }

    Transform transform;

    public AreaAttack(Transform transform)
    {
        this.transform = transform;
    }


    ///<summary>
    ///Get the closest collider of each health entity
    ///</summary>
    public List<CollisionData> RefineHits(RaycastHit[] hits)
    {
        Collider[] colliders = new Collider[30];

        int limit = Mathf.Min(30, hits.Length);
        for (int i = 0; i < 30 && i < limit; i++)
            colliders[i] = hits[i].collider;

        return RefineHits(colliders);
    }

    ///<summary>
    ///Get the closest collider of each health entity
    ///</summary>
    public List<CollisionData> RefineHits(Collider[] colliders)
    {
        List<CollisionData> enemies = new List<CollisionData>(30);

        foreach (Collider collider in colliders)
        {
            if (!collider || !collider.GetComponent<Damageable>())
                continue;

            if (enemies.Exists(e => e.health == collider.GetComponent<Damageable>().GetHealth()))
            {
                CollisionData cd = enemies.Find(e => e.health == collider.GetComponent<Damageable>().GetHealth());
                if ((cd.closestCollider.ClosestPoint(transform.position) - transform.position).magnitude >
                    (collider.ClosestPoint(transform.position) - transform.position).magnitude)
                    cd.closestCollider = collider;
            }
            else
                enemies.Add(new CollisionData(collider.GetComponent<Damageable>().GetHealth(), collider));
        }

        return enemies;
    }
}
