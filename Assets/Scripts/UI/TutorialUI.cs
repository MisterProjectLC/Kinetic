using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public GameTrigger trigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger.OnTriggerActivate += ShowControls;
    }

    void ShowControls()
    {
        GetComponent<Animator>().Play("TutorialControls");
    }
}
