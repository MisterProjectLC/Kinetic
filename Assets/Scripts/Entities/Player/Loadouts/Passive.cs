using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive : Upgrade
{
    protected void Awake()
    {
        Type = "Passive";
        gameObject.SetActive(false);
    }
}
