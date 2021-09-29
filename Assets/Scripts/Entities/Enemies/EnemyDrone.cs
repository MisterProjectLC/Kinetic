using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDrone : MonoBehaviour
{
    Transform playerTransform;
    Enemy enemy;
    const float leeway = 1f;

    [SerializeField]
    float MotorSpeed = 1f;

    [SerializeField]
    float MinimumDistance = 3f;

    private void Start()
    {
        playerTransform = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
        enemy = GetComponent<Enemy>();
        enemy.OnActiveUpdate += OnActiveUpdate;
    }

    void OnActiveUpdate()
    {
        Vector3 playerDistance = playerTransform.position - enemy.Model.transform.position;

        // Check if inside field of view
        if (Vector3.Dot(enemy.Model.transform.forward, playerDistance) < 0f)
            return;


        // Check if view is obstructed
        Ray ray = new Ray(enemy.Model.transform.position, playerTransform.position - enemy.Model.transform.position);
        Physics.Raycast(ray, out RaycastHit hit, 100f, enemy.GroundLayers, QueryTriggerInteraction.Ignore);
        if (hit.collider && (hit.distance < playerDistance.magnitude))
            return;

        playerDistance = Vector3.ProjectOnPlane(playerDistance, Vector3.up);
        // X movement
        if (playerDistance.magnitude > MinimumDistance)
            transform.position += playerDistance.normalized * MotorSpeed * Time.deltaTime;

        // Y movement
        if (!DetectGroundOverPlayer())
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, playerTransform.position.y, transform.position.z), 0.4f * Time.deltaTime);
    }

    bool DetectGroundOverPlayer()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, transform.position.y, enemy.GroundLayers, QueryTriggerInteraction.Ignore);
        return hit.point.y > playerTransform.position.y;
    }

}
