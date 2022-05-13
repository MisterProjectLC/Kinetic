using System;
using UnityEngine;

public abstract class ValueVariable<T> : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
    [SerializeField]
    T DebugValue;
#endif
    public T Value { get; protected set; }    
    public T InitialValue;
    public bool DisableInitialValue = false;
    public Action OnChange;

    void OnEnable()
    {
        if (!DisableInitialValue)
        {
            SetValue(InitialValue);
        }
    }

    public void SetValue(T value)
    {
#if UNITY_EDITOR
        DebugValue = value;
#endif
        Value = value;
        OnChange?.Invoke();
    }

    public void SetValue(ValueVariable<T> value)
    {
        SetValue(value.Value);
    }

    public void SubscribeToChange(Action subscriber)
    {
        OnChange += subscriber;
    }

    public abstract void ApplyChange(T amount);

    public abstract void ApplyChange(ValueVariable<T> amount);

    public static implicit operator T(ValueVariable<T> variable)
    {
        return variable.Value;
    }
}
