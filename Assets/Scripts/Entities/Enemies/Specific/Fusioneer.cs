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
    EnemyWeaponsManager weaponsManager;
    Transform playerTransform;

    float cooldown = 0.05f;
    float clock = 0f;
    bool chargingLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        weaponsManager = GetComponent<EnemyWeaponsManager>();
        animator = GetComponent<Animator>();

        rocketLauncher = weaponsManager.GetWeapons()[0];
        LaserCannon = weaponsManager.GetWeapons()[1];
        GyroCannon = weaponsManager.GetWeapons()[2];
        LaserPointer = weaponsManager.GetWeapons()[3];

        rocketLauncher.SubscribeToFire(FireAnimation);
        LaserCannon.SubscribeToFire(LaserShot);

        crystalShield.OnDeactivate += PhaseTwo;
        GetComponent<Health>().OnCriticalLevel += PhaseThree;

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
            enemy.ReceiveForce(rocketLauncher.BackwardsForce * -rocketLauncher.Mouth.transform.forward);
    }

    void LaserShot(Weapon weapon)
    {
        weaponsManager.OnlyShootIfPlayerInView = false;
        chargingLaser = false;
        LaserPointer.SetOffCooldown();
        LaserPointer.GetComponent<AudioSource>().Stop();
    }
}
