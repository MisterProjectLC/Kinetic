using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walljump : MonoBehaviour
{
    PlayerCharacterController playerController;

    float m_LastTimeWallAirTouched = 0f;
    [SerializeField]
    const float k_WallAirDetectionTime = 0.2f;
    Vector3 wallDirection;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerCharacterController>();
        playerController.OnJumpAir += Execute;
        playerController.OnCollision += OnWallCollision;
    }

    private void Update()
    {
        RaycastHit? hit = playerController.WallCheck();
        if (hit != null)
            TouchWall(hit.Value.normal);
    }

    public void OnWallCollision(ControllerColliderHit hit)
    {
        TouchWall(hit.normal);
    }

    public void TouchWall(Vector3 normal)
    {
        m_LastTimeWallAirTouched = Time.time;
        wallDirection = normal;
    }

    public void Execute()
    {
        if (OnWallAir())
        {
            Vector3 faceDirection = playerController.PlayerCamera.transform.forward;
            playerController.GetComponent<AudioSource>().Play();
            playerController.MoveVelocity = (Vector3.up + new Vector3(wallDirection.x, 0f, wallDirection.z).normalized + 
                new Vector3(faceDirection.x * 1.5f, faceDirection.y > 0f ? faceDirection.y : 0f, faceDirection.z * 1.5f)) *
                playerController.JumpForce * 1.5f;
        }
    }

    public bool OnWallAir()
    {
        return Time.time <= m_LastTimeWallAirTouched + k_WallAirDetectionTime;
    }
}
