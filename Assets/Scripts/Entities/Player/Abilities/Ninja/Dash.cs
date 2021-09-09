using System.Collections;
using UnityEngine;
public class Dash : Ability
{
    [Tooltip("Dash intensity")]
    public float DashSpeed = 50f;

    [Tooltip("Dash duration")]
    public float DashDuration = 0.25f;

    PlayerCharacterController player;
    PlayerInputHandler input;
    Attack attack;

    bool dashing = false;

    public void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        player.OnTrigger += OnTrigger;
        input = GetComponentInParent<PlayerInputHandler>();
        attack = GetComponent<Attack>();
        attack.OnKill += ResetCooldown;
    }

    public override void Execute()
    {
        dashing = true;
        player.gameObject.layer = LayerMask.NameToLayer("Shifted");
        player.MoveControlEnabled = false;
        player.MoveVelocity = DashSpeed * player.PlayerCamera.transform.TransformVector(input.GetMoveInput());
        StartCoroutine("EndDash");
    }


    IEnumerator EndDash()
    {
        yield return new WaitForSeconds(DashDuration);
        dashing = false;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
        player.MoveControlEnabled = true;
        player.MoveVelocity = Vector3.zero;
    }


    private void OnTrigger(Collider other)
    {
        if (dashing)
            attack.AttackTarget(other.gameObject);
    }
}
