using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTrigger : MonoBehaviour
{
    [SerializeField]
    bool OneShot = false;
    bool oneshotted = false;

    public List<GameTrigger> blockers;
    public List<GameTrigger> resetters;
    int blockerCount = 0;

    public UnityAction OnFreeOfBlockers;
    public UnityAction OnResetOneshot;
    public UnityAction OnTriggerActivate;
    public UnityAction OnTriggerDestroy;

    private void Awake()
    {
        foreach (GameTrigger gameTrigger in blockers)
        {
            if (gameTrigger != null)
            {
                gameTrigger.OnTriggerDestroy += RemoveBlocker;
                blockerCount++;
            }
        }

        foreach (GameTrigger gameTrigger in resetters)
        {
            if (gameTrigger != null)
                gameTrigger.OnTriggerActivate += ResetOneShot;
        }
    }

    void RemoveBlocker()
    {
        blockerCount--;
        if (blockerCount == 0)
            OnFreeOfBlockers?.Invoke();
    }

    void ResetOneShot()
    {
        if (blockerCount <= 0)
        {
            oneshotted = false;
            OnResetOneshot?.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (blockerCount > 0)
            return;

        OnTriggerDestroy?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (blockerCount > 0)
            return;

        if (oneshotted)
            return;

        if (OneShot)
            oneshotted = true;

        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();
        OnTriggerActivate?.Invoke();
    }
}
