using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float clock = 0f;

    [SerializeField]
    Weapon weapon;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Weapon
        clock += Time.deltaTime;
        if (clock > weapon.FireCooldown)
        {
            clock = 0f;
            weapon.Fire();
        }

        // Gravity
        //MoveVelocity += Vector3.down * Constants.Gravity * GravityMultiplier * Time.deltaTime;
        //Move(MoveVelocity * Time.deltaTime);
    }
}
