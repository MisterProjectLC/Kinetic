using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketeer : MonoBehaviour
{
    [SerializeField]
    float StaggerDuration = 5f;
    [SerializeField]
    StatusEffect StaggerEffect;

    [SerializeField]
    float maxDistance = 30f;

    Weapon weapon;
    Animator animator;
    Enemy enemy;
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<IEnemyWeaponsManager>().GetWeapons()[0];
        weapon.SubscribeToFire(FireAnimation);
        GetComponent<Health>().OnCriticalLevel += Stagger;
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        playerTransform = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
    }

    void Stagger()
    {
        enemy.GravityMultiplier = 2;
        enemy.ReceiveStatusEffect(StaggerEffect, StaggerDuration);
        StartCoroutine(EndStagger());
    }

    IEnumerator EndStagger()
    {
        yield return new WaitForSeconds(StaggerDuration);
        enemy.GravityMultiplier = 0;
    }

    void FireAnimation(Weapon weapon)
    {
        animator.SetTrigger("Fire");
        //if ((playerTransform.position - enemy.Model.transform.position).magnitude <= maxDistance)
        //    enemy.ReceiveForce(weapon.BackwardsForce * -weapon.Mouth.transform.forward);
    }
}
