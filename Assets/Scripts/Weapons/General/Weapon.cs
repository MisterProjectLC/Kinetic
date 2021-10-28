using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Where the bullets exit from")]
    public Transform Mouth;

    [Header("General")]
    public string DisplayName = "";

    public ObjectManager.PoolableType BulletType;

    [Tooltip("Layer mask")]
    public LayersConfig HitLayers;

    [Header("Hitscan")]
    [Tooltip("Max distance to which the bullets travel")]
    [SerializeField]
    private float MaxDistance = 100f;

    [Tooltip("Prefab containing hit animation")]
    [SerializeField]
    private GameObject Sparks;

    [Header("Projectile")]
    [SerializeField]
    private GameObject Projectile;
    [HideInInspector]
    public List<GameObject> ActiveProjectiles;

    [Header("Attributes")]
    [Tooltip("Pushback force when firing the gun")]
    [SerializeField]
    private float backwardsForce = 10f;
    public float BackwardsForce { get { return backwardsForce; } }

    [Tooltip("Max angle at which the bullets spread from the center")]
    [Range(0f, 10f)]
    [SerializeField]
    private float MaxSpreadAngle = 0f;

    [Tooltip("Amount of bullets sent")]
    [SerializeField]
    private int BulletCount = 12;

    [Tooltip("If enabled, holding the button engages repeated shooting")]
    public bool Automatic = false;
    public float FireCooldown = 0f;
    float clock = 0f;

    public float InitialFireCooldown = 0f;

    public UnityAction OnFire;

    private void Start()
    {
        clock = InitialFireCooldown;
        ActiveProjectiles = new List<GameObject>(BulletCount);
        MaxSpreadAngle /= 100f;
    }

    private void Update()
    {
        if (clock > 0f)
            clock -= Time.deltaTime;
    }

    public void Trigger()
    {
        if (clock <= 0f)
        {
            Fire();
            clock = FireCooldown;
        }
    }

    public void Fire()
    {
        if (OnFire != null)
            OnFire.Invoke();

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
                instance.GetComponent<Projectile>().Setup(direction, HitLayers.layers, GetComponentInParent<Actor>().gameObject);
                if (instance.GetComponent<Attack>() && GetComponent<Attack>() && GetComponent<Attack>().OnKill != null)
                    instance.GetComponent<Attack>().OnKill += GetComponent<Attack>().OnKill;
                instance.GetComponent<Projectile>().OnDestroy += RemoveProjectileFromList;
                ActiveProjectiles.Add(instance);

            // Hitscan attack
            } else { 
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
    }

    private void RemoveProjectileFromList(Projectile proj)
    {
        proj.gameObject.GetComponent<Projectile>().OnDestroy -= RemoveProjectileFromList;
        ActiveProjectiles.Remove(proj.gameObject);
    }
}
