using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumppad : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private Vector3 jumpDirection;
    [SerializeField]
    private LayerMask detectLayers;

    float clock = 0f;
    const float cooldown = 0.5f;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(meshRenderer.bounds.center, new Vector3(1f, 1f, 2f), 
            Quaternion.identity, detectLayers);
        if (colliders.Length > 0 && clock <= 0f)
        {
            clock = cooldown;
            foreach (Collider collider in colliders)
                ApplyJump(collider.gameObject);
        }

        if (clock > 0f)
            clock -= Time.deltaTime;
    }

    private void ApplyJump(GameObject target)
    {
        Vector3 worldDirection = transform.TransformDirection(jumpDirection);

        if (target.GetComponent<PlayerCharacterController>())
        {
            target.GetComponent<PlayerCharacterController>().MoveVelocity = worldDirection;
            target.GetComponent<PlayerCharacterController>().IsGrounded = false;
            target.GetComponent<PlayerCharacterController>().transform.position += Vector3.up;
        }
        else if (target.GetComponentInParent<PlayerCharacterController>())
        {
            target.GetComponentInParent<PlayerCharacterController>().MoveVelocity = worldDirection;
            target.GetComponentInParent<PlayerCharacterController>().IsGrounded = false;
            target.GetComponentInParent<PlayerCharacterController>().transform.position += Vector3.up;
        }


        else if (target.GetComponentInParent<Enemy>())
            target.GetComponentInParent<Enemy>().ReceiveKnockback(worldDirection);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(meshRenderer.bounds.center, new Vector3(1f, 1f, 2f));
    }
}
