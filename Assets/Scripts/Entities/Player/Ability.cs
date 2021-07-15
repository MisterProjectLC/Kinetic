using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [Tooltip("Cooldown")]
    public float Cooldown = 5f;
    private float Timer = 0f;

    public bool HoldAbility = false;

    private void Update()
    {
        Timer = Timer > 0f ? Timer - Time.deltaTime : 0f;
    }

    public void Activate()
    {
        if (Timer <= 0f)
        {
            Timer = Cooldown;
            Execute();
        }
    }

    public abstract void Execute();
}
