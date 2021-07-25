using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("How much time it takes to update the player's supposed position")]
    [SerializeField]
    private float updateCooldown = 0.1f;

    [Tooltip("How radically does the object rotate to match")]
    [Range(0f, 1f)] [SerializeField]
    private float lerpSpeed = 0.5f;

    private Vector3 oldTargetPosition;
    private Vector3 newTargetPosition;
    private float clock = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Transform cameraTransform = ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform;
        oldTargetPosition = cameraTransform.position;
        newTargetPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Constant rotation
        Vector3 currentTargetPosition = Vector3.Slerp(oldTargetPosition, newTargetPosition, lerpSpeed);
        oldTargetPosition = currentTargetPosition;
        transform.LookAt(currentTargetPosition);

        // Update targetting
        clock += Time.deltaTime;
        if (clock > updateCooldown)
        {
            clock = 0f;
            Transform cameraTransform = ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform;
            newTargetPosition = cameraTransform.position;
        }
    }
}
