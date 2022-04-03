using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock
{
    float clock;
    float target;

    public Clock(float target)
    {
        this.clock = 0f;
        this.target = target;
    }

    /// <summary>
    /// Advances the clock by time. If the clock rings, returns true.
    /// </summary>
    /// <returns></returns>
    public bool TickAndRing(float time)
    {
        Tick(time);
        return Ring();
    }

    public void Tick(float time)
    {
        clock += time;
    }

    public bool CheckIfRing()
    {
        return clock >= target;
    }

    public bool Ring()
    {
        if (CheckIfRing())
        {
            clock = 0f;
            return true;
        }

        return false;
    }
}
