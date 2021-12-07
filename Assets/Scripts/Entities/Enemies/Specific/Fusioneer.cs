using UnityEngine;

public class Fusioneer : MonoBehaviour
{
    [SerializeField]
    Weapon LaserCannon;
    [SerializeField]
    Weapon GyroCannon;


    [SerializeField]
    float maxDistance = 30f;

    [SerializeField]
    Transform RocketLauncherDownTarget;
    [SerializeField]
    Transform LaserCannonDownTarget;

    Weapon weapon;
    Animator animator;
    Enemy enemy;
    CrystalShield crystalShield;
    Transform playerTransform;

    float cooldown = 0.2f;
    float clock = 0f;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<Enemy>().weapons[0];
        weapon.OnFire += FireAnimation;
        GetComponent<Health>().OnCriticalLevel += ActivateGyro;
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        playerTransform = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;
        if (clock > cooldown)
        {
            clock = 0f;
            /*RocketLauncher.GetComponentInParent<FaceTarget>().TargetPosition =
                LaserCannon.FireCooldown / 4 < LaserCannon.GetClock() ?
                ActorsManager.Player.GetComponentInChildren<Camera>().transform : RocketLauncherDownTarget;
            */
            LaserCannon.GetComponentInParent<FaceTarget>().TargetPosition =
                3*LaserCannon.FireCooldown / 4 > LaserCannon.GetClock() ?
                ActorsManager.Player.GetComponentInChildren<Camera>().transform : LaserCannonDownTarget;
        }
    }


    void ActivateGyro()
    {
        animator.SetBool("Gyro", true);
        GyroCannon.FireCooldown = 0.01f;
    }

    void FireAnimation()
    {
        animator.SetTrigger("Fire");
        if ((playerTransform.position - enemy.Model.transform.position).magnitude <= maxDistance)
            enemy.ReceiveKnockback(weapon.BackwardsForce * -weapon.Mouth.transform.forward);
    }
}
