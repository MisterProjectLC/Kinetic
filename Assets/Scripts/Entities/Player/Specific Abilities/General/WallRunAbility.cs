using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunAbility : Passive
{
    public float wallMaxDistance = 1;
    public float wallSpeedMultiplier = 1.2f;
    public float minimumHeight = 1.2f;
    public float maxAngleRoll = 20;
    [Range(0.0f, 1.0f)]
    public float normalizedAngleThreshold = 0.1f;

    public float jumpDuration = 1;
    public float wallBouncing = 3;
    public float cameraTransitionDuration = 1;

    public float wallGravityDownForce = 20f;

    PlayerCharacterController player;
    WallRun wallRun;

    new void Awake()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        wallRun = player.GetComponentInChildren<WallRun>();
        base.Awake();
    }

    private void OnEnable()
    {
        player.OnJumpAir += wallRun.OnJump;
        player.OnCollision += OnTouchWall;
    }

    private void OnDisable()
    {
        player.OnJumpAir -= wallRun.OnJump;
        player.OnCollision -= OnTouchWall;
    }

    void OnTouchWall(ControllerColliderHit hit)
    {
        wallRun.OnTouchWall(hit.point, hit.normal);
    }
}
