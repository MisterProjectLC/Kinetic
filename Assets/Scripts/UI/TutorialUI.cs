using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public GameTrigger trigger;
    public GameTrigger hideTrigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger.OnTriggerActivate += ShowControls;
        trigger.OnTriggerActivate += HideControls;
    }

    void ShowControls()
    {
        GetComponent<Animator>().Play("TutorialControls");
    }

    void HideControls()
    {
        gameObject.SetActive(false);
    }
}
