using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dash : Ability
{
    [Tooltip("Dash intensity")]
    public float DashSpeed = 10f;

    [Tooltip("Dash duration")]
    public float DashDuration = 0.5f;

    [Tooltip("Dash damage")]
    public int DashDamage = 2;

    PlayerCharacterController player;
    PlayerInputHandler input;

    bool dashing = false;

    public void Start()
    {
        player = GetComponent<PlayerCharacterController>();
        input = GetComponent<PlayerInputHandler>();
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


    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable && dashing)
        {
            if (damageable.GetAffiliation() != player.gameObject.GetComponent<Actor>().Affiliation)
            {
                Debug.Log("DAMAGE");
                damageable.InflictDamage(DashDamage);
            }
        }
    }
}
