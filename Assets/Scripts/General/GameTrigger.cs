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

    private void Start()
    {
        StartCoroutine("StartAuto");
    }

    IEnumerator StartAuto()
    {
        yield return new WaitForSeconds(0.01f);
        if (AutoStart)
            OnTriggerActivate?.Invoke();
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

    public bool IsOneshot()
    {
        return OneShot;
    }
}
