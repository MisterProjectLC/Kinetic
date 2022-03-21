using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaPassive : MonoBehaviour
{
    bool crit = false;
    WeaponAbility ability;

    public void Start()
    {
        ability = GetComponent<WeaponAbility>();
        //GetComponentInParent<StyleMeter>().SubscribeToCritical(OnCritical);
        GetComponent<Attack>().OnAttack += (GameObject g, float f, int i) => CritReset();
        //GetComponent<Attack>().OnKill += (Attack a, GameObject g, bool b) => ability.ResetCooldown();
    }

    void OnCritical(bool critical)
    {
        crit = critical;
    }

    void CritReset()
    {
        ability.ResetCooldown();
    }
}
