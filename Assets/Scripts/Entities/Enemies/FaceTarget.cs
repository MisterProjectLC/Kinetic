using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("How much time it takes to update the player's supposed position")]
    [SerializeField]
    private float updateCooldown = 0.1f;

    [Tooltip("How fast does the object rotate to face the target")]
    [SerializeField]
    private float turnSpeed = 10f;

    private Quaternion currentPosition;
    private Vector3 newTargetPosition;
    private float clock = 0f;

    // Start is called before the first frame update
    void Start()
    {
        newTargetPosition = ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform.position;

        GetComponent<Enemy>().OnActiveUpdate += OnActiveUpdate;
    }

    // Update is called once per frame
    void OnActiveUpdate()
    {
        // Constant rotation
        transform.rotation = Quaternion.RotateTowards(currentPosition, Quaternion.LookRotation(newTargetPosition - transform.position),
            turnSpeed*Time.deltaTime);
        currentPosition = transform.rotation;

        // Update targetting
        clock += Time.deltaTime;
        if (clock > updateCooldown)
        {
            clock = 0f;
            newTargetPosition = ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform.position;
        }
    }
}
