using UnityEngine;

public class Minigun : MonoBehaviour
{
    [SerializeField]
    float MaxSlowdown = 0.5f;
    float slowdown = 0f;

    float emergencyResetTimer = 0.5f;
    float clock = 0f;

    WeaponAbility weaponAbility;
    PlayerCharacterController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        weaponAbility = GetComponentInParent<WeaponAbility>();
        weaponAbility.OnExecute += Execute;
    }


    void Update()
    {
        if (slowdown < 1f)
        {
            clock += Time.deltaTime;
            if (clock > emergencyResetTimer)
            {
                slowdown = 1f;
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
                slowdown -= 0.1f;
        }
        else
            slowdown = 1f;
        player.SetSlowdown(slowdown, "minigun");
        //weaponAbility.Execute(Ability.Input.ButtonDown);
    }
}
