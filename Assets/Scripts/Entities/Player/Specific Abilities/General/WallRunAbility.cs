using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunAbility : Passive
{
    WallRun wallRun;

    new void Awake()
    {
        wallRun = GetComponentInParent<PlayerCharacterController>().GetComponentInChildren<WallRun>();
        base.Awake();
    }

    private void OnEnable()
    {
        wallRun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        wallRun.gameObject.SetActive(false);
    }
}
