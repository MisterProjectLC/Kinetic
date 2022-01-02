using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    Transform playerTransform;
    Enemy enemy;
    const float leeway = 1f;

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
    float clock = 0f;
    bool stopped = false;

    private void Start()
    {
        playerTransform = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
        enemy = GetComponent<Enemy>();
        enemy.OnActiveUpdate += OnActiveUpdate;
    }

    void OnActiveUpdate()
    {
        if (!enemy.IsPlayerInView())
            return;

        clock += Time.deltaTime;
        if (myTrigger && clock > enemyCollisionCooldown)
        {
            clock = 0f;
            RaycastHit[] hitInfos = Physics.SphereCastAll(enemy.Model.transform.position + 
                enemy.Model.transform.forward*1.5f, 0.23f, enemy.Model.transform.forward, enemy.GetCollisionDistance(), 
                LayerMask.GetMask("EnemyTrigger"));

            foreach (RaycastHit hit in hitInfos)
                if (hit.collider != myTrigger)
                {
                    stopped = true;
                    Debug.Log("Stopped");
                    return;
                }

            stopped = false;
        }

        if (stopped)
            return;

        Vector3 playerDistance = playerTransform.position - enemy.Model.transform.position;
        playerDistance = Vector3.ProjectOnPlane(playerDistance, Vector3.up);

        // X movement
        if (playerDistance.magnitude > MinimumDistance)
            transform.position += playerDistance.normalized * MotorSpeed * Time.deltaTime;

        // Y movement
        if (!(DetectTooCloseWall() ||DetectTooCloseGroundOrCeiling()))
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, playerTransform.position.y, transform.position.z), VerticalSpeed * Time.deltaTime);
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
        Physics.Raycast(ray, out RaycastHit hit, MaximumProximityToGround, enemy.GroundLayers.layers, QueryTriggerInteraction.Ignore);
        return hit.collider;
    }
}
