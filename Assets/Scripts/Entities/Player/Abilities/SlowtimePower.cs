using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowtimePower : MonoBehaviour
{
    PlayerInputHandler input;
    bool slowdown = false;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        if (input.GetSlowtime())
        {
            slowdown = !slowdown;
            if (slowdown)
                Time.timeScale = 0.25f;
            else
                Time.timeScale = 1f;
        }
    }
}
