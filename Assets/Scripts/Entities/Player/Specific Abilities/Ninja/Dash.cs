using System.Collections;
using UnityEngine;

public class Dash : Ability
{
    [Tooltip("Dash intensity")]
    public float DashSpeed = 50f;

    [Tooltip("Dash duration")]
    public float DashDuration = 0.25f;

    PlayerCharacterController player;
    PlayerInputHandler inputHandler;
    Attack attack;

    ExtraJump extraJump;

    bool dashing = false;

    public void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        player.OnTrigger += OnTrigger;
        inputHandler = GetComponentInParent<PlayerInputHandler>();
        attack = GetComponent<Attack>();
        attack.OnKill += (Attack a, GameObject g, bool b) => ResetCooldown();
    }

    public override void Execute(Input input)
    {
        if (extraJump)
            extraJump.ResetJump();
        else
            extraJump = player.GetComponentInChildren<ExtraJump>();

        dashing = true;
        player.gameObject.layer = LayerMask.NameToLayer("Shifted");
        player.MoveControlEnabled = false;
        player.SetMoveVelocity(DashSpeed * player.GetPlayerCamera().transform.TransformVector(inputHandler.GetMoveInput()));
        StartCoroutine(EndDash());
    }


    IEnumerator EndDash()
    {
        yield return new WaitForSeconds(DashDuration);
        dashing = false;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
        player.MoveControlEnabled = true;
        player.SetMoveVelocity(Vector3.zero);
    }


    private void OnTrigger(Collider other)
    {
        if (dashing)
            attack.AttackTarget(other.gameObject);
    }
}
