using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    Transform playerTransform;
    Enemy enemy;
    const float leeway = 1f;

    [SerializeField]
    [Range(0f, 1f)]
    float VerticalSpeed = 0.4f;

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
        if (!enemy.IsPlayerInView())
            return;

        Vector3 playerDistance = playerTransform.position - enemy.Model.transform.position;
        playerDistance = Vector3.ProjectOnPlane(playerDistance, Vector3.up);

        // X movement
        if (playerDistance.magnitude > MinimumDistance)
            transform.position += playerDistance.normalized * MotorSpeed * Time.deltaTime;

        // Y movement
        if (!DetectGroundOverPlayer())
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, playerTransform.position.y, transform.position.z), VerticalSpeed * Time.deltaTime);
    }

    bool DetectGroundOverPlayer()
    {
        Ray ray = new Ray(enemy.Model.transform.position, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, transform.position.y, enemy.GroundLayers.layers, QueryTriggerInteraction.Ignore);
        return hit.point.y > playerTransform.position.y;
    }
}
