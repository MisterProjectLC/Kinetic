using UnityEngine;

public class FloatBar : ValueBar<float>
{
    [SerializeField]
    FloatReference CurrentValue;
    [SerializeField]
    FloatReference MaxValue;

    new protected void Awake()
    {
        SetReferences(CurrentValue, MaxValue);
        base.Awake();
    }

    public override void UpdateBar()
    {
        if (bar)
            bar.sizeDelta = barSize * new Vector2(Mathf.Clamp01(CurrentValue / MaxValue), 1f);
    }
}

