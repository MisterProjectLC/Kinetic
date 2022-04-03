using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFallHandler
{
    public void FallFatal(float VerticalLimit, Transform FallRespawnPoint);
}
