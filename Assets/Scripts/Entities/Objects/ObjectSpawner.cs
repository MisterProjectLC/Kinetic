using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject Spawnee;
    ObjectManager.PoolableType? objectType = null;

    [SerializeField]
    Transform DespawnPoint;

    List<GameObject> children = new List<GameObject>(10);

    [SerializeField]
    float SpawnRadius = 0f;
    [SerializeField]
    int ChildLimit = 100;
    [SerializeField]
    float SpawnTimer = 5f;
    [SerializeField]
    float SpawnStartDelay = 0f;
    float clock = 0f;

    [SerializeField]
    bool AvoidPlayerLoS = false;
    Transform playerTransform;
    [SerializeField]
    LayersConfig ViewBlockedLayers;

    private void Start()
    {
        clock = SpawnStartDelay;

        playerTransform = ActorsManager.AM.GetPlayer().transform;

        if (Spawnee.GetComponent<Poolable>())
            objectType = Spawnee.GetComponent<Poolable>().Type;
    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;
        if (clock > SpawnTimer)
        {
            clock = 0f;

            // Clear
            for (int i = 0; i < children.Count; i++)
                if (children[i] == null)
                {
                    children.RemoveAt(i);
                    i--;
                }

            // DespawnPoint
            if (DespawnPoint != null)
                for (int i = 0; i < children.Count; i++)
                {
                    if (Vector3.Dot(DespawnPoint.position - children[i].transform.position, transform.position - children[i].transform.position) > 0f)
                    {
                        Destroy(children[i]);
                        children.RemoveAt(i);
                        i--;
                    }
                }

            if (AvoidPlayerLoS && IsPlayerInView())
                return;

            if (children.Count >= ChildLimit)
                return;

            GameObject newInstance;
            Vector3 randomOffset = Random.insideUnitSphere * SpawnRadius;
            if (objectType != null)
            {
                newInstance = ObjectManager.OM.SpawnObjectFromPool((ObjectManager.PoolableType)objectType, Spawnee);
                newInstance.transform.SetParent(transform);
                newInstance.transform.position = transform.position + randomOffset;
                newInstance.transform.rotation = transform.rotation;
            }
            else
                newInstance = Instantiate(Spawnee, transform.position + randomOffset, transform.rotation);

            newInstance.transform.SetParent(transform);
            if (newInstance.GetComponent<NavMeshAgent>())
                newInstance.GetComponent<NavMeshAgent>().Warp(transform.position + randomOffset);

            children.Add(newInstance);
        }
    }

    bool IsPlayerInView()
    {
        Vector3 playerDistance = playerTransform.position - transform.position;

        // Check if inside field of view
        if (Vector3.Dot(transform.forward, playerDistance) < 0f)
            return false;

        // Check if view is obstructed
        Ray ray = new Ray(transform.position, playerTransform.position - transform.position);
        Physics.Raycast(ray, out RaycastHit hit, 100f, ViewBlockedLayers.layers, QueryTriggerInteraction.Ignore);
        if (hit.collider && (hit.distance < playerDistance.magnitude))
            return false;

        return true;
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, SpawnRadius + 0.05f);
    }
}
