using System;

[Serializable]
public abstract class ValueReference<T>
{
    public bool UseConstant = true;
    public T ConstantValue;

    public abstract void SubscribeToChange(Action subscriber);
}

