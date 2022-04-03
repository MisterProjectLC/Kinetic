using UnityEngine;
using SmartData.SmartFloat;

public class FloatBar : ValueBar
{
    [SerializeField]
    FloatReader CurrentValue;
    [SerializeField]
    FloatReader MaxValue;

    private void Start()
    {
        CurrentValue.BindListener(UpdateBar);
        CurrentValue.UnbindUnityEventOnDestroy();
    }
    public void UpdateBar()
    {
        if (bar)
            bar.sizeDelta = barSize * new Vector2(Mathf.Clamp01(CurrentValue / MaxValue), 1f);
    }
}

