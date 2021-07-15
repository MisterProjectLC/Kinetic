using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrone : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform);
    }
}
