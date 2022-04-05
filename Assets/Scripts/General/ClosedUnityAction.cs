using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ParentClosedUnityAction<T>
{
    UnityAction<T> Action;


    public void Subscribe(UnityAction<T> subscriber)
    {
        Action += subscriber;
    }

    public void Unsubscribe(UnityAction<T> subscriber)
    {
        Action -= subscriber;
    }
}

public class ClosedUnityAction<T>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
