using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketeer : MonoBehaviour
{
    [SerializeField]
    float StaggerDuration = 5f;

    [SerializeField]
    float maxDistance = 30f;

    [SerializeField]
    Weapon weapon;

    Animator animator;
    FlyingEnemy flyingEnemy;
    Enemy enemy;
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        weapon.OnFire += FireAnimation;
        GetComponent<Health>().OnCriticalLevel += Stagger;
        animator = GetComponent<Animator>();

        flyingEnemy = GetComponent<FlyingEnemy>();
        enemy = GetComponent<Enemy>();
        playerTransform = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
    }

    void Stagger()
    {
        enemy.GravityMultiplier = 2;
        enemy.ReceiveStun(StaggerDuration);
        StartCoroutine("EndStagger");
    }

    IEnumerator EndStagger()
    {
        yield return new WaitForSeconds(StaggerDuration);
        enemy.GravityMultiplier = 0;
    }

    void FireAnimation()
    {
        animator.SetTrigger("Fire");
        if ((playerTransform.position - enemy.Model.transform.position).magnitude <= maxDistance)
            enemy.ReceiveKnockback(weapon.BackwardsForce * -weapon.Mouth.transform.forward);
    }
}
