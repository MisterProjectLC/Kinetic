using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walljump : MonoBehaviour
{
    PlayerCharacterController playerController;

    float m_LastTimeWallAirTouched = 0f;
    const float k_WallAirDetectionTime = 0.2f;
    Vector3 wallDirection;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerCharacterController>();
        playerController.OnJumpAir += Execute;
    }

    private void Update()
    {
        RaycastHit? hit = playerController.WallCheck();
        if (hit != null)
        {
            m_LastTimeWallAirTouched = Time.time;
            wallDirection = hit.Value.normal;
        }
    }

    public void Execute()
    {
        if (OnWallAir())
        {
            playerController.MoveVelocity = (Vector3.up + new Vector3(wallDirection.x, 0f, wallDirection.z).normalized) *
                playerController.JumpForce * 1.5f;
        }
    }

    public bool OnWallAir()
    {
        return Time.time <= m_LastTimeWallAirTouched + k_WallAirDetectionTime;
    }
}
