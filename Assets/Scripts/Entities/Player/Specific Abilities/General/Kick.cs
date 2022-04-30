using System.Collections.Generic;
using UnityEngine;

public class Kick : Ability
{
    [SerializeField]
    GameObject armature;

    [SerializeField]
    LayersConfig EnemyLayers;

    [SerializeField]
    int DistancePerIncrease = 50;

    PlayerInputHandler inputHandler;
    PlayerCharacterController player;
    Attack[] attacks;
    AreaAttack areaAttack;
    Animator animator;

    Vector3 halfExtents;

    public void Start()
    {
        inputHandler = GetComponentInParent<PlayerInputHandler>();
        attacks = GetComponents<Attack>();
        animator = GetComponent<Animator>();
        areaAttack = new AreaAttack(transform);
        player = GetComponentInParent<PlayerCharacterController>();
        halfExtents = new Vector3(2f, 2f, 2f);

        OnUpdate += Updater;
    }


    private void Updater()
    {
        if (inputHandler.GetKick())
            Activate(Input.ButtonDown);
    }

    public override void Execute(Input input)
    {
        armature.SetActive(true);
        animator.SetTrigger("Kick");

        Camera playerCamera = player.GetPlayerCamera();

        RaycastHit[] hits = Physics.BoxCastAll(playerCamera.transform.position + playerCamera.transform.forward, halfExtents,
            playerCamera.transform.forward, Quaternion.identity, 5f, EnemyLayers.layers, QueryTriggerInteraction.Collide);

        List<AreaAttack.CollisionData> enemies = areaAttack.RefineHits(hits);

        foreach (AreaAttack.CollisionData enemy in enemies)
            foreach (Attack attack in attacks)
                attack.AttackTarget(enemy.closestCollider.gameObject, Mathf.Max(1, (int)player.MoveVelocity.magnitude/ DistancePerIncrease));
    }
}

