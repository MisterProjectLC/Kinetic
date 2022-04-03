using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAnchor : Ability
{
    PlayerCharacterController player;

    // Start is called before the first frame update
    public void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
    }

    public override void Execute(Input input)
    {
        player.SetMoveVelocity(Vector3.zero);
    }
}
