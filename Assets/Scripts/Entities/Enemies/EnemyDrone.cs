using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDrone : MonoBehaviour
{
    PlayerCharacterController player;
    Enemy enemy;
    NavMeshAgent meshAgent;
    const float leeway = 1f;

    private void Start()
    {
        player = ActorsManager.Player.GetComponent<PlayerCharacterController>();
        enemy = GetComponent<Enemy>();
        meshAgent = GetComponent<NavMeshAgent>();
        enemy.OnActiveUpdate += OnActiveUpdate;
    }

    void OnActiveUpdate()
    {
        if (enemy.Model.transform.localPosition.y < 0f)
            enemy.Model.transform.localPosition = Vector3.zero;
        else
            enemy.Model.transform.position = Vector3.Lerp(enemy.Model.transform.position,
                new Vector3(enemy.Model.transform.position.x, player.PlayerCamera.transform.position.y, enemy.Model.transform.position.z),
                0.4f * Time.deltaTime);

        DetectNewGround();
    }

    void DetectNewGround()
    {
        
        Ray ray = new Ray(enemy.Model.transform.position, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, enemy.Model.transform.localPosition.y, enemy.GroundLayers, QueryTriggerInteraction.Ignore);

        if (Mathf.Abs(hit.point.y - enemy.transform.position.y) > leeway)
        {
            Debug.Log("Warping");
            Vector3 oldPosition = new Vector3(enemy.Model.transform.position.x, enemy.Model.transform.position.y, enemy.Model.transform.position.z);
            meshAgent.Warp(hit.point);
            enemy.Model.transform.position = oldPosition;
        }
        UpdateHeight();
    }

    void UpdateHeight()
    {
        enemy.HoverHeight = enemy.Model.transform.position.y;
        //meshAgent.height = enemy.HoverHeight + 1;
    }
}
