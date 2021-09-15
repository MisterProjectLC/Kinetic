using UnityEngine;

public class Minigun : MonoBehaviour
{
    [SerializeField]
    float MaxSlowdown = 0.5f;
    float slowdown = 0f;

    WeaponAbility weaponAbility;
    PlayerCharacterController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        weaponAbility = GetComponentInParent<WeaponAbility>();
        weaponAbility.OnExecute += Execute;
    }


    private void Update()
    {
        player.SetSlowdown(1f - slowdown);
        if (slowdown > 0f)
            slowdown -= Time.deltaTime;
    }


    public void Execute()
    {
        if (slowdown < MaxSlowdown)
            slowdown += 0.2f;
        weaponAbility.Execute();
    }
}
