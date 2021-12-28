using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicShield : EnemyShield
{
    [SerializeField]
    float Cooldown = 4f;
    [SerializeField]
    float Duration = 1f;

    float clock = 0f;
    bool shieldUp = false;

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;

        if ((clock >= Duration && shieldUp) || (clock >= Cooldown && !shieldUp))
        {
            clock = 0f;
            shieldUp = !shieldUp;
            SetShield(shieldUp);
        }
    }
}
