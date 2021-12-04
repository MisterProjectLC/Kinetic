using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameTrigger[] Buttons = new GameTrigger[] { };
    [SerializeField]
    private string OpenAnimation = "DoorOpen";
    [SerializeField]
    bool ActivableByID = true;

    private void Awake()
    {
        if (GetComponent<UniqueID>() && ActivableByID)
            GetComponent<UniqueID>().OnObjectRegistered += Activate;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameTrigger Button in Buttons)
            Button.OnTriggerActivate += Activate;
    }

    // Update is called once per frame
    void Activate()
    {
        GetComponent<Animator>().Play(OpenAnimation);
        if (GetComponent<UniqueID>() && ActivableByID)
            GetComponent<UniqueID>().RegisterID();
    }
}
