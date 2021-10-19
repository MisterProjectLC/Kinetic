using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Health[] Turrets;
    int turretsRemaining = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Health turret in Turrets)
        {
            turret.OnDie += OnTurretDestroyed;
            turretsRemaining++;
        }
    }

    void OnTurretDestroyed()
    {
        turretsRemaining--;

        if (turretsRemaining <= 0)
            GetComponent<Health>().Kill();
    }
}
