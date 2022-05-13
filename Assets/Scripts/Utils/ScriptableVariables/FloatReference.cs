// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------
using System;
using UnityEngine;

[Serializable]
public class FloatReference : ValueReference<float>
{
    [SerializeField]
    FloatVariable Variable;

    public FloatReference()
    {
    }

    public FloatReference(int value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public override void SubscribeToChange(Action subscriber)
    {
        if (!UseConstant)
            Variable.SubscribeToChange(subscriber);
    }

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set
        {
            if (UseConstant)
                ConstantValue = value;
            else
                Variable.SetValue(value);
        }
    }

    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}

