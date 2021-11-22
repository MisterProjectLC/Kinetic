using UnityEngine;

public class WeaponSlowdown : MonoBehaviour
{
    [SerializeField]
    float MaxSlowdown = 0.5f;
    float slowdown = 0f;

    [SerializeField]
    float SlowdownRate = 0.05f;

    [SerializeField]
    float emergencyResetTimer = 0.2f;
    float clock = 0f;

    WeaponAbility weaponAbility;
    PlayerCharacterController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        weaponAbility = GetComponentInParent<WeaponAbility>();
        weaponAbility.OnExecute += Execute;
    }

    private void OnDisable()
    {
        slowdown = 1f;
        player.SetSlowdown(1f, "weapon");
        clock = 0f;
    }


    void Update()
    {
        if (slowdown < 1f)
        {
            clock += Time.deltaTime;
            if (clock > emergencyResetTimer)
            {
                slowdown = 1f;
                player.SetSlowdown(1f, "weapon");
                clock = 0f;
            }
        }
    }


    public void Execute(Ability.Input input)
    {
        if (input == Ability.Input.ButtonDown)
        {
            clock = 0f;
            if (slowdown > MaxSlowdown)
                slowdown -= SlowdownRate;
        }
        else
            slowdown = 1f;

        player.SetSlowdown(slowdown, "weapon");
    }
}
