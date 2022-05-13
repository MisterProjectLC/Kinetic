using UnityEngine;

[CreateAssetMenu(fileName = "Stun", menuName = "StatusEffect/Stun")]
public class Stun : StatusEffect
{

    public override void ApplyEffect(StatusEffectReceiver target)
    {
        /*if (target.GravityMultiplier < 1f)
        {
            lastGravityMultiplier = target.GravityMultiplier;
            target.GravityMultiplier = 1f;
        }
        if (pathAgent && pathAgent.isOnNavMesh)
            pathAgent.isStopped = true;*/
    }
}
