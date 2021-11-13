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

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<GameTrigger>();
        if (trigger.blockers.Count > 0)
            gameObject.GetComponentInChildren<Renderer>().material = disabledMaterial;

        trigger.OnTriggerActivate += Disable;
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
