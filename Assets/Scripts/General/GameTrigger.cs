using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTrigger : MonoBehaviour
{
    int blockerCount = 0;

    public UnityAction OnTriggerActivate;
    public UnityAction OnTriggerDestroy;

    private void Awake()
    {
        if (GetComponent<AudioSource>())
            OnTriggerActivate += GetComponent<AudioSource>().Play;

    }

    private void OnDestroy()
    {
        OnTriggerDestroy?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerActivate?.Invoke();
    }
}
