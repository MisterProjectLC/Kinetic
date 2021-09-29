using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movepad : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private LayerMask detectLayers;

    [Header("Stats")]
    [SerializeField]
    private Vector3 detectSize = new Vector3(1f, 1f, 2f);
    [SerializeField]
    private Vector3 detectPosOffset = Vector3.zero;
    [SerializeField]
    private Vector3 moveDirection;
    [SerializeField]
    private bool isJump = true;

    float clock = 0f;
    float cooldown { get {
            if (isJump)
                return 0.5f;
            else
                return 0.05f;
        } 
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(meshRenderer.bounds.center + detectPosOffset, detectSize, 
            Quaternion.identity, detectLayers);
        if (colliders.Length > 0 && clock <= 0f)
        {
            clock = cooldown;
            foreach (Collider collider in colliders)
                ApplyMove(collider.gameObject);
        }

        if (clock > 0f)
            clock -= Time.deltaTime;
    }

    private void ApplyMove(GameObject target)
    {
        Vector3 worldDirection = transform.TransformDirection(moveDirection);

        if (target.GetComponent<PlayerCharacterController>() || target.GetComponentInParent<PlayerCharacterController>())
        {
            PlayerCharacterController player = target.GetComponent<PlayerCharacterController>();
            if (!player)
                player = target.GetComponentInParent<PlayerCharacterController>();

            if (!player.IsGrounded && !isJump)
                return;

            if (!isJump)
                player.ApplyForce(worldDirection);
            else
            {
                player.MoveVelocity = worldDirection;
                player.IsGrounded = false;
                player.transform.position += Vector3.up;
            }
        }

        else if (target.GetComponentInParent<Enemy>())
            target.GetComponentInParent<Enemy>().ReceiveKnockback(worldDirection);
    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(meshRenderer.bounds.center + detectPosOffset, detectSize);
    }
}
