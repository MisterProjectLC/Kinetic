using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTrigger : MonoBehaviour
{
    [SerializeField]
    bool AutoStart = false;
    [SerializeField]
    bool OneShot = false;
    bool oneshotted = false;

    public List<GameTrigger> blockers;
    public List<GameTrigger> resetters;
    int blockerCount = 0;

    public UnityAction OnFreeOfBlockers;
    public UnityAction OnResetOneshot;


    UnityAction OnTriggerActivate;
    public void SubscribeToTriggerActivate(UnityAction subscriber) { OnTriggerActivate += subscriber; }
    public void UnsubscribeToTriggerActivate(UnityAction subscriber) { OnTriggerActivate -= subscriber; }
    public UnityAction OnTriggerDestroy;

    private void Awake()
    {
        foreach (GameTrigger gameTrigger in blockers)
        {
            if (gameTrigger != null)
            {
                gameTrigger.OnTriggerActivate += RemoveBlocker;
                blockerCount++;
            }
        }

        foreach (GameTrigger gameTrigger in resetters)
        {
            if (gameTrigger != null)
                gameTrigger.OnTriggerActivate += ResetOneShot;
        }
    }

    private void Start()
    {
        StartCoroutine("StartAuto");
    }

    IEnumerator StartAuto()
    {
        yield return new WaitForSeconds(0.01f);
        if (AutoStart)
            Activate();
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
        Destroy();
    }

    public void Activate()
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

    public void Destroy()
    {
        if (blockerCount > 0)
            return;

        OnTriggerDestroy?.Invoke();
    }

    public bool IsOneshot()
    {
        return OneShot;
    }
}
