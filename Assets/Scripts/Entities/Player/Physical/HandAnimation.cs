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
                playerCharacter.IsGrounded && playerCharacter.MoveVelocity.magnitude > 4f);
    }
}
