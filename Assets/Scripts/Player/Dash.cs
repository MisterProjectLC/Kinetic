using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dash : Ability
{
    [Tooltip("Dash intensity")]
    public float DashSpeed = 10f;

    [Tooltip("Dash duration")]
    public float DashDuration = 0.5f;

    PlayerCharacterController player;
    PlayerInputHandler input;

    public void Start()
    {
        player = GetComponent<PlayerCharacterController>();
        input = GetComponent<PlayerInputHandler>();
    }

    public override void Execute()
    {
        player.MoveControlEnabled = false;
        player.MoveVelocity = DashSpeed * player.PlayerCamera.transform.TransformVector(input.GetMoveInput());
        Debug.Log("Teste1");
        StartCoroutine("EndDash");
        Debug.Log("Teste3");
    }


    IEnumerator EndDash()
    {
        Debug.Log("Teste");
        yield return new WaitForSeconds(DashDuration);
        player.MoveControlEnabled = true;
        player.MoveVelocity = Vector3.zero;
        Debug.Log("Teste2");
    }
}
