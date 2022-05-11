using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class FlyingEnemy : MonoBehaviour
{
    Transform playerTransform;
    Enemy enemy;
    IEnemyPhysics physics;

    [Header("Y Movement")]
    [SerializeField]
    [Range(0f, 1f)]
    float VerticalSpeed = 0.4f;

    [SerializeField]
    float MaximumProximityToGround = 1f;

    [Header("X Movement")]
    [SerializeField]
    float MotorSpeed = 1f;

    [SerializeField]
    float MaximumProximityToWall = 1f;

    [SerializeField]
    float MinimumDistance = 3f;

    [SerializeField]
    Collider myTrigger;

    float enemyCollisionCooldown = 1f;
    Clock tickClock;
    Clock collisionClock;

    RaycastHit[] hitInfos;
    RaycastHit[] hits;
    bool stopped = false;

    const float TIME_UNTIL_FRAME_UPDATE = 0.04f;

    private void Start()
    {
        tickClock = new Clock(TIME_UNTIL_FRAME_UPDATE);
        collisionClock = new Clock(enemyCollisionCooldown);
        hitInfos = new RaycastHit[20];
        hits = new RaycastHit[1];

        playerTransform = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
        enemy = GetComponent<Enemy>();
        physics = GetComponent<IEnemyPhysics>();
        enemy.SubscribeToUpdate(OnUpdate);
    }

    void OnUpdate()
    {
        CheckOtherFlyingEnemiesCollision();
        FlyingMovement();
    }

    void CheckOtherFlyingEnemiesCollision()
    {
        if (collisionClock.TickAndRing(Time.deltaTime) && myTrigger)
        {
            Physics.SphereCastNonAlloc(enemy.Model.transform.position + enemy.Model.transform.forward * 1.5f,
                0.23f, enemy.Model.transform.forward, hitInfos, physics.GetCollisionDistance(), LayerMask.GetMask("EnemyTrigger"));

            foreach (RaycastHit hit in hitInfos)
                if (hit.collider && hit.collider != myTrigger)
                {
                    stopped = true;
                    return;
                }

            stopped = false;
        }
    }

    void FlyingMovement()
    {
        if (stopped || !tickClock.TickAndRing(Time.deltaTime) || !enemy.IsPlayerInView())
            return;

        Vector3 playerDistance = playerTransform.position - enemy.Model.transform.position;
        playerDistance = Vector3.ProjectOnPlane(playerDistance, Vector3.up);

        //if (DetectTooCloseWall())
        //    return;

        // X movement
        if (!DetectTooCloseWall() && playerDistance.magnitude > MinimumDistance)
            transform.position += playerDistance.normalized * MotorSpeed * TIME_UNTIL_FRAME_UPDATE;

        // Y movement
        if (DetectTooCloseGroundOrCeiling())
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, playerTransform.position.y, transform.position.z), VerticalSpeed * TIME_UNTIL_FRAME_UPDATE);
    }


    bool DetectTooCloseWall()
    {
        if (MaximumProximityToWall <= 0)
            return false;

        Ray ray = new Ray(enemy.Model.transform.position, playerTransform.position - enemy.Model.transform.position);
        Physics.Raycast(ray, out RaycastHit hit, MaximumProximityToWall, enemy.GroundLayers.layers, QueryTriggerInteraction.Ignore);
        return hit.collider;
    }


    bool DetectTooCloseGroundOrCeiling()
    {
        if (MaximumProximityToGround <= 0)
            return false;

        Ray ray = new Ray(enemy.Model.transform.position, Vector3.up * 
            (enemy.Model.transform.position.y < playerTransform.position.y ? 1f : -1f));
        Physics.RaycastNonAlloc(ray, hits, MaximumProximityToGround, enemy.GroundLayers.layers, QueryTriggerInteraction.Ignore);
        return hits[0].collider;
    }
}
