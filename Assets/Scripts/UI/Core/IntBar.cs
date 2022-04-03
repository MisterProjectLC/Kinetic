using UnityEngine;
using SmartData.SmartInt;

public class IntBar : ValueBar
{
    [SerializeField]
    IntReader CurrentValue;
    [SerializeField]
    IntReader MaxValue;

    private void Start()
    {
        CurrentValue.BindListener(UpdateBar);
        CurrentValue.UnbindUnityEventOnDestroy();
    }

    public void UpdateBar()
    {
        if (bar)
            bar.sizeDelta = barSize * new Vector2(Mathf.Clamp01((float)CurrentValue / MaxValue), 1f);
    }
}
