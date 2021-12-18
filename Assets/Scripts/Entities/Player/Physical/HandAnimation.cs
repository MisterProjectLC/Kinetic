using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    [SerializeField]
    PlayerCharacterController playerCharacter;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animator>().SetBool("Walking",
                playerCharacter.IsGrounded && Vector3.ProjectOnPlane(playerCharacter.MoveVelocity, Vector3.up).magnitude > 4f);
    }
}
