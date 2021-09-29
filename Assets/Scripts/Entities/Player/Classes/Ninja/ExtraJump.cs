using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraJump : MonoBehaviour
{
    PlayerCharacterController playerController;
    PlayerInputHandler input;
    Walljump walljump;

    bool jumpAvailable = true;
    bool takenCareOf = false;

    private void Start()
    {
        input = GetComponentInParent<PlayerInputHandler>();
        playerController = GetComponentInParent<PlayerCharacterController>();
        playerController.OnJumpAir += Execute;
        
        walljump = playerController.gameObject.GetComponentInChildren<Walljump>();
    }

    private void Update()
    {
        if (playerController.IsGrounded)
            jumpAvailable = true;
    }

    public void Execute()
    {
        if (!walljump)
            walljump = playerController.gameObject.GetComponentInChildren<Walljump>();

        if (walljump && walljump.OnWallAir())
            jumpAvailable = true;

        else if (takenCareOf)
            takenCareOf = false;

        else if (jumpAvailable)
        {
            jumpAvailable = false;
            Vector3 jumpSpeed = playerController.PlayerCamera.transform.TransformVector(input.GetMoveInput());
            playerController.MoveVelocity = new Vector3(jumpSpeed.x * playerController.JumpForce * 1.25f, playerController.JumpForce * 1.25f, 
                jumpSpeed.z * playerController.JumpForce * 1.25f);
            //playerController.Jump();
            foreach (ExtraJump jump in playerController.gameObject.GetComponentsInChildren<ExtraJump>())
                if (jump != this)
                    jump.NotifyTakenCareOf();
            
        }
    }

    public void ResetJump()
    {
        jumpAvailable = true;
    }

    // Used when multiple extra jumps are present and only one must be activated
    public void NotifyTakenCareOf()
    {
        takenCareOf = true;
    }
}
