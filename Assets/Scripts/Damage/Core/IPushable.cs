using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    public void ReceiveForce(Vector3 force, Attack attack, bool isSticky = false);
}
