using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflect : SecondaryAbility
{
    [SerializeField]
    LayersConfig layers; 

    PlayerCharacterController player;
    Attack attack;
    Actor actor;
    Animator animator;

    Vector3 halfExtents;

    [SerializeField]
    AudioClip deflectSound;

    public void Start()
    {
        attack = GetComponent<Attack>();
        player = GetComponentInParent<PlayerCharacterController>();
        actor = player.GetComponent<Actor>();
        halfExtents = new Vector3(2f, 2f, 2f);
    }

    public override void Execute(Input input)
    {
        if (!animator)
            animator = ParentAbility.GetComponent<Animator>();
        animator.SetTrigger("Deflect");

        RaycastHit[] hits = Physics.BoxCastAll(player.PlayerCamera.transform.position + player.PlayerCamera.transform.forward, halfExtents, 
            player.PlayerCamera.transform.forward, Quaternion.identity, 5f, layers.layers, QueryTriggerInteraction.Collide);

        int hitCount = 0;
        foreach (RaycastHit hit in hits)
        {
            Debug.Log("Deflect Hit: " + hit.collider.gameObject.name);

            Projectile proj = hit.collider.GetComponentInParent<Projectile>();
            if (!proj)
                continue;

            proj.MoveVelocity = proj.MoveVelocity.magnitude * 2 * player.PlayerCamera.transform.forward;
            hitCount++;

            Attack theirAttack = proj.GetComponent<Attack>();
            if (theirAttack)
            {
                theirAttack.Damage *= 3;
                theirAttack.Agressor = actor;
                attack.SetupClone(theirAttack);
            }
            ResetCooldown();
        }

        StartCoroutine(DeflectAnim(hitCount));
    }

    IEnumerator DeflectAnim(int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            animator.SetTrigger("Shoot");
            PlaySound(deflectSound);
            float oldTime = Time.timeScale;
            Time.timeScale = 0.1f;

            yield return new WaitForSecondsRealtime(0.1f);
            Time.timeScale = oldTime;
        }
    }
}