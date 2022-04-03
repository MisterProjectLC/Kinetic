using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, Entity
{
    [SerializeField]
    bool debug = false;

    SightlineChecker sightlineChecker;

    [Header("References")]
    [SerializeField]
    public GameObject Model;

    public LayersConfig GroundLayers;
    public LayersConfig ViewBlockedLayers;

    public Transform PlayerTransform { get; private set; }

    IEnemyPhysics EnemyPhysics;
    StatusEffectManager statusEffectManager;

    public bool Movable { get { return !(EnemyPhysics is StaticPhysics); } }
    public float GravityMultiplier { get { return EnemyPhysics.GravityMultiplier; } set { EnemyPhysics.GravityMultiplier = value; } }

    UnityAction OnUpdate;
    public void SubscribeToUpdate(UnityAction subscriber) { OnUpdate += subscriber; }
    public UnityAction<Vector3> OnKnockbackCollision;

    [HideInInspector]
    public bool IsGrounded = true;

    protected void Awake()
    {
        EnemyPhysics = GetComponent<IEnemyPhysics>();
        EnemyPhysics.Setup(Model);

        statusEffectManager = new StatusEffectManager();
    }

    void Start()
    {
        PlayerTransform = ActorsManager.AM.GetPlayerCamera().transform;
        sightlineChecker = new SightlineChecker(Model.transform, ViewBlockedLayers.layers);
    }

    // Update is called once per frame
    void Update()
    {
        EnemyPhysics.OnUpdate();
        OnUpdate?.Invoke();
    }


    public bool IsPlayerInView()
    {
        return sightlineChecker.IsTargetInWatchersView(PlayerTransform);
    }

    public RaycastHit RayToGround()
    {
        return EnemyPhysics.RayToGround();
    }

    public void FallFatal(float VerticalLimit, Transform FallRespawnPoint)
    {
        Debug.Log("Enemy fall");
        Health enemyHealth = GetComponent<Health>();
        if (enemyHealth)
            enemyHealth.Kill();
    }

    public void ReceiveForce(Vector3 force, bool sticky = false)
    {
        EnemyPhysics.ReceiveForce(force, sticky);
    }

    public void ReceiveMotion(Vector3 motion)
    {
        EnemyPhysics.ReceiveMotion(motion);
    }

    public void WarpPosition(Vector3 newPosition)
    {
        EnemyPhysics.WarpPosition(newPosition);
    }

    public void SetMoveVelocity(Vector3 velocity)
    {
        EnemyPhysics.SetMoveVelocity(velocity);
    }

    public Vector3 GetMoveVelocity()
    {
        return EnemyPhysics.GetMoveVelocity();
    }

    public void ReceiveStatusEffect(StatusEffect statusEffect, float duration)
    {
        statusEffectManager.ReceiveStatusEffect(statusEffect, duration);
    }

    public bool HasStatusEffect(StatusEffect statusEffect)
    {
        return statusEffectManager.HasStatusEffect(statusEffect);
    }

    public bool HasAnyOfTheseStatusEffects(List<StatusEffect> statusEffects)
    {
        return statusEffectManager.HasAnyOfTheseStatusEffects(statusEffects);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}