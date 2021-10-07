using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transitions : MonoBehaviour
{
    Animator animator;

    UnityAction OnAnimationEnd;
    List<UnityAction> subscribers = new List<UnityAction>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayTransition(string transitionName, UnityAction OnEnd)
    {
        PlayTransition(transitionName);
        OnAnimationEnd += OnEnd;
        subscribers.Add(OnEnd);
    }

    public void PlayTransition(string transitionName)
    {
        animator.Play(transitionName);
    }


    public void TransitionEnded()
    {
        if (OnAnimationEnd == null)
            return;
        OnAnimationEnd.Invoke();
        foreach (UnityAction a in subscribers)
            OnAnimationEnd -= a;
        subscribers.Clear();

    }
}
