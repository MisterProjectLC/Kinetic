using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField]
    Material disabledMaterial;
    [SerializeField]
    Material enabledMaterial;

    GameTrigger trigger;

    private void Awake()
    {
        trigger = GetComponent<GameTrigger>();
        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += trigger.Activate;

        if (trigger.blockers.Count > 0)
            gameObject.GetComponentInChildren<Renderer>().material = disabledMaterial;

        if (trigger.IsOneshot())
            trigger.SubscribeToTriggerActivate(Disable);
        trigger.OnFreeOfBlockers += Enable;
        trigger.OnResetOneshot += Enable;
    }


    private void Disable()
    {
        gameObject.GetComponentInChildren<Renderer>().material = disabledMaterial;
    }


    private void Enable()
    {
        gameObject.GetComponentInChildren<Renderer>().material = enabledMaterial;
    }

}
