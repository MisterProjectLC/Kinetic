using UnityEngine;

public class Paralyze : StatusEffect
{
    [SerializeField]
    float ParalyzeDuration = 5f;

    protected override void ApplyEffect(Enemy target)
    {
        target.ReceiveParalyze(ParalyzeDuration);
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
