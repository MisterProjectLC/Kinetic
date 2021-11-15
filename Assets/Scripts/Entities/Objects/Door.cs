using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameTrigger Button;
    [SerializeField]
    private string OpenAnimation = "DoorOpen";

    private void Awake()
    {
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += Activate;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Button)
            Button.OnTriggerActivate += Activate;
    }

    // Update is called once per frame
    void Activate()
    {
        GetComponent<Animator>().Play(OpenAnimation);
        GetComponent<UniqueID>()?.RegisterID();
    }
}
