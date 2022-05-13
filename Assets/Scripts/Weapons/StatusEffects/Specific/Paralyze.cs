using UnityEngine;

[CreateAssetMenu(fileName = "Paralyze", menuName = "StatusEffect/Paralyze")]
public class Paralyze : StatusEffect
{
    public override void ApplyEffect(StatusEffectReceiver target)
    {
        //target.ReceiveParalyze(ParalyzeDuration);
    }
}
