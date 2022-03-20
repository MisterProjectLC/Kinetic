using UnityEngine;

public class Fusioneer : MonoBehaviour
{
    Weapon rocketLauncher;
    Weapon LaserCannon;
    Weapon GyroCannon;
    Weapon LaserPointer;

    [SerializeField]
    float maxDistance = 30f;

    [SerializeField]
    Transform RocketLauncherDownTarget;
    [SerializeField]
    Transform LaserCannonDownTarget;

    [SerializeField]
    CrystalShield crystalShield;
    Animator animator;
    Enemy enemy;
    Transform playerTransform;

    float cooldown = 0.05f;
    float clock = 0f;
    bool chargingLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        rocketLauncher = GetComponent<Enemy>().weapons[0];
        LaserCannon = GetComponent<Enemy>().weapons[1];
        GyroCannon = GetComponent<Enemy>().weapons[2];
        LaserPointer = GetComponent<Enemy>().weapons[3];

        rocketLauncher.SubscribeToFire(FireAnimation);
        LaserCannon.SubscribeToFire(LaserShot);

        crystalShield.OnDeactivate += PhaseTwo;
        GetComponent<Health>().OnCriticalLevel += PhaseThree;
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
            if (LaserCannon.FireCooldown* 3f/4 < LaserCannon.GetClock())
                LaserCannon.GetComponentInParent<FaceTarget>().TargetPosition = LaserCannonDownTarget;
            else
                LaserCannon.GetComponentInParent<FaceTarget>().TargetPosition =
                    ActorsManager.Player.GetComponentInChildren<Camera>().transform;
        }

        if (LaserCannon.GetClock() < LaserCannon.FireCooldown / 4)
        {
            if (!chargingLaser)
            {
                LaserPointer.GetComponent<AudioSource>().Play();
                chargingLaser = true;
            }
            LaserPointer.ResetClock();
        }
    }

    void PhaseTwo()
    {
        rocketLauncher.ResetClock();
    }

    void PhaseThree()
    {
        animator.SetBool("Gyro", true);
        GyroCannon.ResetClock();
        GyroCannon.FireCooldown = 0.01f;

        crystalShield.ReactivateEverything();
    }


    void FireAnimation(Weapon weapon)
    {
        animator.SetTrigger("Fire");
        if ((playerTransform.position - enemy.Model.transform.position).magnitude <= maxDistance)
            enemy.ReceiveKnockback(rocketLauncher.BackwardsForce * -rocketLauncher.Mouth.transform.forward);
    }

    void LaserShot(Weapon weapon)
    {
        GetComponent<Enemy>().OnlyShootIfPlayerInView = false;
        chargingLaser = false;
        LaserPointer.SetOffCooldown();
        LaserPointer.GetComponent<AudioSource>().Stop();
    }
}
