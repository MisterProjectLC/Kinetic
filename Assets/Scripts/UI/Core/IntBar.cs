using UnityEngine;

public class IntBar : ValueBar<int>
{
    [SerializeField]
    IntReference CurrentValue;
    [SerializeField]
    IntReference MaxValue;

    new protected void Awake()
    {
        SetReferences(CurrentValue, MaxValue);
        base.Awake();
    }

    public override void UpdateBar()
    {
        if (bar)
            bar.sizeDelta = barSize * new Vector2(Mathf.Clamp01((float)CurrentValue / MaxValue), 1f);
    }
}
