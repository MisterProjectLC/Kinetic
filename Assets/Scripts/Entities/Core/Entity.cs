using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Entity : PhysicsEntity, StatusEffectReceiver
{
    public abstract void FallFatal(float VerticalLimit, Transform FallRespawnPoint);

}
