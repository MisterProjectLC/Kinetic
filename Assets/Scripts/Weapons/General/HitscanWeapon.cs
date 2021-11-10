using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanWeapon : Weapon
{
    [Header("Hitscan")]
    [Tooltip("Max distance to which the bullets travel")]
    [SerializeField]
    float MaxDistance = 100f;

    [Tooltip("Prefab containing hit animation")]
    [SerializeField]
    GameObject Sparks;

    public override void Shoot(Vector3 direction)
    {
        Ray ray = new Ray(Mouth.position, direction);
        Physics.Raycast(ray, out RaycastHit hit, MaxDistance, HitLayers.layers);
        if (hit.collider)
        {
            Vector3 randomVector = Random.insideUnitSphere;
            while (Vector3.Dot(randomVector, hit.normal) == 0)
                randomVector = Random.insideUnitSphere;

            if (Sparks != null)
            {
                GameObject newObject = ObjectManager.OM.SpawnObjectFromPool(ObjectManager.PoolableType.LaserSparks, Sparks);
                newObject.transform.position = hit.point;
                newObject.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Random.insideUnitSphere, hit.normal),
                    hit.normal);
            }
            GetComponent<Attack>().AttackTarget(hit.collider.gameObject);
        }
    }



}