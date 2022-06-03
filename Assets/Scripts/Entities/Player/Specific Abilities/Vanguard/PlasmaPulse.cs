using System.Collections.Generic;
using UnityEngine;

public class PlasmaPulse : SecondaryAbility
{
    [SerializeField]
    ParticleSystem plasmaParticles;

    [SerializeField]
    LayersConfig ProjLayers;

    [SerializeField]
    LayersConfig EnemyLayers;

    PlayerCharacterController player;
    Attack attack;
    AreaAttack areaAttack;

    Vector3 halfExtents;

    public void Start()
    {
        attack = GetComponent<Attack>();
        areaAttack = new AreaAttack(transform);
        player = GetComponentInParent<PlayerCharacterController>();
        halfExtents = new Vector3(2f, 2f, 2f);
    }


    public override void Execute(Input input)
    {
        plasmaParticles.Play();

        Camera playerCamera = player.GetPlayerCamera();

        RaycastHit[] hits = Physics.BoxCastAll(playerCamera.transform.position + playerCamera.transform.forward, halfExtents,
            playerCamera.transform.forward, Quaternion.identity, 5f, ProjLayers.layers, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hits) {
            Poolable poolable = hit.collider.GetComponent<Poolable>();
            if (poolable)
                ObjectManager.EraseObject(poolable);
            else
                Destroy(hit.collider.gameObject);
        }

        hits = Physics.BoxCastAll(playerCamera.transform.position + playerCamera.transform.forward, halfExtents,
            playerCamera.transform.forward, Quaternion.identity, 5f, EnemyLayers.layers, QueryTriggerInteraction.Collide);

        List<AreaAttack.CollisionData> enemies = areaAttack.RefineHits(hits);

        foreach (AreaAttack.CollisionData enemy in enemies)
            attack.AttackTarget(enemy.closestCollider.gameObject);
    }
}

