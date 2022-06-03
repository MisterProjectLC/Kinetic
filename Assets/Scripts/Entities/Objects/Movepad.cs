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
            moveVector = transform.InverseTransformVector(DespawnPoint.localPosition - SpawnPoint.localPosition).normalized * Speed;
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
        detectorCenter = DespawnPoint ? (DespawnPoint.localPosition + SpawnPoint.localPosition) /2 : meshRenderer.bounds.center + detectPosOffset;
        Collider[] colliders = Physics.OverlapBox(detectorCenter, detectSize, meshRenderer.transform.rotation, detectLayers);
        if (colliders.Length > 0)
        {
            if (clock.CheckIfRing())
            {
                List<PhysicsEntity> entities = ExtractEntitiesFromColliders(colliders);
                if (entities.Count > 0)
                    clock.Ring();

                foreach (PhysicsEntity entity in entities)
                    ApplyMove(entity.GetGameObject());
            }
        }

        clock.Tick(Time.deltaTime);
    }

    void ApplyMove(GameObject target)
    {
        Vector3 currentMoveVector = GetMoveDirection();

        PhysicsEntity entity = target.GetComponentInParent<PhysicsEntity>();
        if (isJump)
            entity.SetMoveVelocity(currentMoveVector);
        
        else if (Sticky)
            entity.ReceiveForce(currentMoveVector, Sticky);

        else
            entity.ReceiveMotion(currentMoveVector);
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
