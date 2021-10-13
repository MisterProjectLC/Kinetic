using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Health Button;

    // Start is called before the first frame update
    void Start()
    {
        if (Button)
            Button.OnDamage += Activate;
    }

    // Update is called once per frame
    void Activate()
    {
        GetComponent<Animator>().Play("DoorOpen");
    }
}
