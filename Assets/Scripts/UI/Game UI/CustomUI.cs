using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUI : MonoBehaviour
{
    [SerializeField]
    GameTrigger Trigger;
    [SerializeField]
    GameTrigger HideTrigger;

    protected void Start()
    {
        if (Trigger)
        {
            Trigger.OnTriggerActivate += Show;
            Trigger.OnTriggerDestroy += Show;
        }

        if (HideTrigger)
        {
            HideTrigger.OnTriggerActivate += Hide;
            HideTrigger.OnTriggerDestroy += Hide;
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Trigger)
        {
            Trigger.OnTriggerActivate -= Show;
            Trigger.OnTriggerDestroy -= Show;
        }

        if (HideTrigger)
        {
            HideTrigger.OnTriggerActivate -= Hide;
            HideTrigger.OnTriggerDestroy -= Hide;
        }
    }

    void Show()
    {
        if (gameObject)
            gameObject.SetActive(true);
    }

    void Hide()
    {
        if (gameObject)
            gameObject.SetActive(false);
    }
}
