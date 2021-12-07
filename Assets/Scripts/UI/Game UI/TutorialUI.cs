using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : CustomUI
{
    // Start is called before the first frame update
    protected new void Start()
    {
        ActorsManager.AM.GetPlayer().GetComponent<StyleMeter>().DrainActive = false;
        base.Start();
    }

    void OnEnable()
    {
        GetComponent<Animator>().Play("TutorialControls");
    }
}
