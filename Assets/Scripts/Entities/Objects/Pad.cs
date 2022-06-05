using System.Collections.Generic;
using UnityEngine;

public abstract class Pad : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private LayerMask detectLayers;

    [SerializeField]
    protected Transform SpawnPoint;
    [SerializeField]
    protected Transform DespawnPoint;

    [Header("Vectors")]
    [SerializeField]
    private Vector3 detectSize;
    [SerializeField]
    private Vector3 detectPosOffset = Vector3.zero;
    Vector3 detectorCenter;

    [SerializeField]
    private bool overrideAutomaticSize = false;

    [SerializeField]
    protected float cooldown = 0.05f;

    Clock clock;


    protected void Start()
    {
        clock = new Clock(cooldown);

        if (DespawnPoint && SpawnPoint)
        {
            if (!overrideAutomaticSize)
                detectSize = new Vector3(Mathf.Abs(DespawnPoint.localPosition.x - SpawnPoint.localPosition.x) / 2,
                    1f, transform.localScale.z * 1.5f);
        }
        else
        {
            detectSize /= 2;
        }
        //Debug.Log(DespawnPoint.position + ", " + SpawnPoint.position + ", " + moveVector);
    }


    protected void Update()
    {
        detectorCenter = DespawnPoint ? (DespawnPoint.localPosition + SpawnPoint.localPosition) / 2 : meshRenderer.bounds.center + detectPosOffset;
        Collider[] colliders = Physics.OverlapBox(detectorCenter, detectSize, meshRenderer.transform.rotation, detectLayers);
        if (colliders.Length > 0)
        {
            if (clock.CheckIfRing())
            {
                List<PhysicsEntity> entities = ExtractEntitiesFromColliders(colliders);
                if (entities.Count > 0)
                    clock.Ring();

                foreach (PhysicsEntity entity in entities)
                    ApplyEffect(entity.GetGameObject());
            }
        }

        clock.Tick(Time.deltaTime);
    }

    protected abstract void ApplyEffect(GameObject target);

    List<PhysicsEntity> ExtractEntitiesFromColliders(Collider[] colliders)
    {
        List<PhysicsEntity> physicsEntities = new List<PhysicsEntity>();
        foreach (Collider collider in colliders)
        {
            PhysicsEntity entity = collider.GetComponentInParent<PhysicsEntity>();
            if (entity != null && !physicsEntities.Contains(entity))
                physicsEntities.Add(entity);
        }
        return physicsEntities;
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
