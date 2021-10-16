using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameTrigger Button;
    [SerializeField]
    private string OpenAnimation = "DoorOpen";

    // Start is called before the first frame update
    void Awake()
    {
        if (Button)
            Button.OnTriggerActivate += Activate;
    }

    // Update is called once per frame
    void Activate()
    {
        Debug.Log("TriggerActivate");
        GetComponent<Animator>().Play(OpenAnimation);
    }
}
