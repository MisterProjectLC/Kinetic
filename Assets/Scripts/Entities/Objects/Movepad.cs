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

    [SerializeField]
    private Transform SpawnPoint;
    [SerializeField]
    private Transform DespawnPoint;


    [Header("Vectors")]
    [SerializeField]
    private Vector3 detectSize;
    [SerializeField]
    private Vector3 detectPosOffset = Vector3.zero;
    [SerializeField]
    Vector3 moveVector;

    [Header("Stats")]
    public float Speed = 5f;
    [SerializeField]
    private bool isJump = true;
    [SerializeField]
    private bool overrideAutomaticSize = false;

    float clock = 0f;
    float cooldown { get {
            if (isJump)
                return 0.5f;
            else
                return 0.05f;
        } 
    }


    private void Start()
    {
        if (DespawnPoint && SpawnPoint)
        {
            moveVector = (DespawnPoint.position - SpawnPoint.position).normalized * Speed;
            if (!overrideAutomaticSize)
                detectSize = new Vector3((DespawnPoint.position - SpawnPoint.position).magnitude / 2, 1f, 1.5f);
        }
        //Debug.Log(DespawnPoint.position + ", " + SpawnPoint.position + ", " + moveVector);
    }


    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(meshRenderer.bounds.center + detectPosOffset, detectSize,
            meshRenderer.transform.rotation, detectLayers);
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
        if (target.GetComponent<PlayerCharacterController>() || target.GetComponentInParent<PlayerCharacterController>())
        {
            PlayerCharacterController player = target.GetComponent<PlayerCharacterController>();
            if (!player)
                player = target.GetComponentInParent<PlayerCharacterController>();

            if (!player.IsGrounded && !isJump)
                return;

            if (!isJump)
                player.ApplyForce(moveVector);
            else
            {
                player.MoveVelocity = moveVector;
                player.IsGrounded = false;
                player.transform.position += Vector3.up;
            }
        }

        else if (target.GetComponentInParent<Enemy>() && 
            (Vector3.Dot(moveVector, target.GetComponentInParent<Enemy>().GetMoveVelocity()) < 0f || 
            moveVector.magnitude > target.GetComponentInParent<Enemy>().GetMoveVelocity().magnitude))
            target.GetComponentInParent<Enemy>().ReceiveKnockback(moveVector);
    }

    public Vector3 GetMoveDirection()
    {
        return moveVector;
    }

    public Vector3 GetLowerExtremePoint()
    {
        return DespawnPoint.position;
    }

    public Vector3 GetUpperExtremePoint()
    {
        return SpawnPoint.position;
    }


    private void OnDrawGizmos()
    {
        if (DespawnPoint && SpawnPoint)
            Gizmos.DrawLine(GetLowerExtremePoint(), GetUpperExtremePoint());
        Gizmos.DrawCube(meshRenderer.bounds.center + detectPosOffset, detectSize);
    }
}
