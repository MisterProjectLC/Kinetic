using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Tooltip("Where the bullets exit from")]
    public Transform Mouth;

    [Tooltip("Layer mask")]
    public LayerMask HitLayers;

    [Tooltip("Max distance to which the bullets travel")]
    public float MaxDistance = 100f;

    [Header("Attributes")]
    [Tooltip("Pushback force when firing the gun")]
    public float BackwardsForce = 10f;

    [Tooltip("Max angle at which the bullets spread from the center")]
    [Range(0f, 1f)]
    public float MaxSpreadAngle = 0f;

    [Tooltip("Amount of bullets sent")]
    public int BulletCount = 12;

    [Tooltip("Damage each bullet inflicts")]
    public int BulletDamage = 1;

    [Tooltip("If enabled, holding the button engages repeated shooting")]
    public bool Automatic = false;

    [Tooltip("If enabled, holding the button engages repeated shooting")]
    public float FireCooldown = 0f;

    [Tooltip("Prefab containing hit animation")]
    public GameObject Sparks;

    public void Fire()
    {
        for (int i = 0; i < BulletCount; i++)
        {
            float spread = Random.Range(0f, MaxSpreadAngle);
            Vector3 direction = Vector3.Slerp(transform.right, Random.insideUnitSphere, spread);

            Ray ray = new Ray(Mouth.position, direction);
            Physics.Raycast(ray, out RaycastHit hit, MaxDistance, HitLayers);
            if (hit.collider)
            {
                Vector3 randomVector = Random.insideUnitSphere;
                while (Vector3.Dot(randomVector, hit.normal) == 0)
                    randomVector = Random.insideUnitSphere;

                Instantiate(Sparks, hit.point, Quaternion.LookRotation(Vector3.ProjectOnPlane(Random.insideUnitSphere, hit.normal), hit.normal));
                if (hit.collider.gameObject.GetComponent<Damageable>())
                    hit.collider.gameObject.GetComponent<Damageable>().InflictDamage(BulletDamage);
            }
        }
    }
}
