using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "ScriptableVariable/FloatVariable")]
public class FloatVariable : ValueVariable<float>
{
    public override void ApplyChange(float amount)
    {
        SetValue(Value + amount);
    }

    public override void ApplyChange(ValueVariable<float> amount)
    {
        SetValue(Value + amount);
    }
}
