using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Where the bullets exit from")]
    public Transform Mouth;

    [Tooltip("Prefab containing hit animation")]
    public GameObject Sparks;

    [Header("General")]
    public string DisplayName = "";

    public ObjectManager.PoolableType BulletType;

    [Tooltip("Layer mask")]
    public LayerMask HitLayers;

    [Header("Hitscan")]
    [Tooltip("Max distance to which the bullets travel")]
    public float MaxDistance = 100f;

    [Header("Projectile")]
    public GameObject Projectile;

    [Header("Attributes")]
    [Tooltip("Pushback force when firing the gun")]
    public float BackwardsForce = 10f;

    [Tooltip("Max angle at which the bullets spread from the center")]
    [Range(0f, 0.25f)]
    public float MaxSpreadAngle = 0f;

    [Tooltip("Amount of bullets sent")]
    public int BulletCount = 12;

    [Tooltip("If enabled, holding the button engages repeated shooting")]
    public bool Automatic = false;

    [Tooltip("If enabled, holding the button engages repeated shooting")]
    public float FireCooldown = 0f;

    public void Fire()
    {
        for (int i = 0; i < BulletCount; i++)
        {
            // Get Direction
            float spread = Random.Range(0f, MaxSpreadAngle);
            Vector3 direction = Vector3.Slerp(Mouth.forward, Random.insideUnitSphere, spread);

            // Projectile attack
            if (Projectile != null)
            {
                GameObject instance = ObjectManager.OM.SpawnObjectFromPool(BulletType, Projectile).gameObject;
                instance.transform.position = Mouth.position;
                instance.GetComponent<Projectile>().Setup(direction, HitLayers, Sparks);

            // Hitscan attack
            } else { 
                Ray ray = new Ray(Mouth.position, direction);
                Physics.Raycast(ray, out RaycastHit hit, MaxDistance, HitLayers);
                if (hit.collider)
                {
                    Vector3 randomVector = Random.insideUnitSphere;
                    while (Vector3.Dot(randomVector, hit.normal) == 0)
                        randomVector = Random.insideUnitSphere;

                    GameObject newObject = ObjectManager.OM.SpawnObjectFromPool(ObjectManager.PoolableType.LaserSparks, Sparks);
                    newObject.transform.position = hit.point;
                    newObject.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Random.insideUnitSphere, hit.normal), hit.normal);
                    GetComponent<Attack>().InflictDamage(hit.collider.gameObject);
                }
            }
        }
    }
}
