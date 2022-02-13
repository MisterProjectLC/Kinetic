using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaPassive : MonoBehaviour
{
    public void Start()
    {
        GetComponent<Attack>().OnAttack += (GameObject g, float f, int i) => GetComponent<Ability>().ResetCooldown();
    }
}
