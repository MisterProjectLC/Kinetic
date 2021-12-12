using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    [SerializeField]
    float StunDuration = 5f;

    protected override void ApplyEffect(Enemy target)
    {
        target.ReceiveStun(StunDuration);
    }

    protected override void EndEffect(Enemy target)
    {
        return;
    }

    protected override void TickEffect(Enemy target)
    {
        return;
    }
}
