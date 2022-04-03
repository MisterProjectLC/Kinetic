using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallArea : MonoBehaviour
{
    [SerializeField]
    float VerticalLimit;

    [SerializeField]
    Transform FallRespawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponentInParent<Entity>();
        if (entity == null)
            entity = other.GetComponentInChildren<Entity>();

        if (entity != null)
            entity.FallFatal(VerticalLimit, FallRespawnPoint);
        /*
        if (entity)
        {
            other.GetComponentInChildren<Entity>().FallFatal(VerticalLimit, FallRespawnPoint);
            other.GetComponentInChildren<PlayerFallHandler>().VerticalLimit = VerticalLimit;
            other.GetComponentInChildren<PlayerFallHandler>().FallRespawnPoint = FallRespawnPoint.position;
        }

        if (other.GetComponentInParent<Enemy>() && !other.GetComponentInParent<FlyingEnemy>())
        {
            Debug.Log("Enemy fall");
            Health enemyHealth = other.GetComponentInParent<Enemy>().GetComponent<Health>();
            if (enemyHealth)
                enemyHealth.Kill();
        }
        */
    }
}
