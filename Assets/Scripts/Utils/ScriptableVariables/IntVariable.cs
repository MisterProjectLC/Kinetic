using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableVariable/IntVariable")]
public class IntVariable : ValueVariable<int>
{
    public override void ApplyChange(int amount)
    {
        SetValue(Value + amount);
    }

    public override void ApplyChange(ValueVariable<int> amount)
    {
        SetValue(Value + amount);
    }
}
