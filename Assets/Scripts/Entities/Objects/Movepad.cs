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
    Vector3 detectorCenter;

    [Header("Stats")]
    public float Speed = 5f;
    [SerializeField]
    private bool isJump = true;
    public bool Sticky = false;
    [SerializeField]
    private bool overrideAutomaticSize = false;

    [SerializeField]
    private float cooldownOverride = -1f;

    Clock clock;

    float cooldown { get {
            if (isJump)
                return 0.5f;
            else
                return cooldownOverride == -1f ? 0.05f : cooldownOverride;
        } 
    }


    private void Start()
    {
        clock = new Clock(cooldown);

        if (DespawnPoint && SpawnPoint)
        {
            moveVector = transform.InverseTransformVector(DespawnPoint.position - SpawnPoint.position).normalized * Speed;
            if (!overrideAutomaticSize)
                detectSize = new Vector3(Mathf.Abs(DespawnPoint.localPosition.x - SpawnPoint.localPosition.x) / 2, 
                    1f, transform.localScale.z*1.5f);
        }
        else
        {
            detectSize /= 2;
        }
        //Debug.Log(DespawnPoint.position + ", " + SpawnPoint.position + ", " + moveVector);
    }


    private void Update()
    {
        detectorCenter = DespawnPoint ? (DespawnPoint.position + SpawnPoint.position)/2 : meshRenderer.bounds.center + detectPosOffset;
        Collider[] colliders = Physics.OverlapBox(detectorCenter, detectSize, meshRenderer.transform.rotation, detectLayers);
        if (colliders.Length > 0)
        {
            /*foreach (Collider collider in colliders)
                if (Sticky && collider.GetComponent<PlayerCharacterController>())
                    collider.GetComponent<PlayerCharacterController>().IsGrounded = true;*/

            if (clock.CheckIfRing())
            {
                foreach (PhysicsEntity entity in ExtractEntitiesFromColliders(colliders))
                    ApplyMove(entity.GetGameObject());
            }
        }

        clock.Ring();
        clock.Tick(Time.deltaTime);
    }

    void ApplyMove(GameObject target)
    {
        Vector3 currentMoveVector = GetMoveDirection();

        PhysicsEntity entity = target.GetComponentInParent<PhysicsEntity>();
        if (isJump)
            entity.SetMoveVelocity(currentMoveVector);
        else
            entity.ReceiveMotion(currentMoveVector);

        /*
        if (entity.GetComponentInParent<PlayerCharacterController>())
        {
            PlayerCharacterController player = target.GetComponent<PlayerCharacterController>();
            if (!player)
                player = target.GetComponentInParent<PlayerCharacterController>();

            if (!player.IsGrounded && !isJump)
                return;

            if (!isJump)
                player.ReceiveForce(currentMoveVector, Sticky);
            else
            {
                player.MoveVelocity = currentMoveVector;
                player.IsGrounded = false;
                player.transform.position += Vector3.up;
            }
        }

        else if (target.GetComponentInParent<Enemy>() && 
            (Vector3.Dot(currentMoveVector, target.GetComponentInParent<Enemy>().GetMoveVelocity()) < 0f ||
            currentMoveVector.magnitude > target.GetComponentInParent<Enemy>().GetMoveVelocity().magnitude))
            target.GetComponentInParent<Enemy>().ReceiveKnockback(currentMoveVector);
        */
    }

    List<PhysicsEntity> ExtractEntitiesFromColliders(Collider[] colliders) {
        List<PhysicsEntity> physicsEntities = new List<PhysicsEntity>();
        foreach (Collider collider in colliders) {
            PhysicsEntity entity = collider.GetComponentInParent<PhysicsEntity>();
            if (entity != null && !physicsEntities.Contains(entity))
                physicsEntities.Add(entity);
        }
        return physicsEntities;
    }

    public Vector3 GetMoveDirection()
    {
        return moveVector.magnitude * transform.TransformVector(moveVector).normalized;
    }

    public void SetMoveDirection(Vector3 value)
    {
        moveVector = value;
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
