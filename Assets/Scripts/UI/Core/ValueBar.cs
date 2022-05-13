using UnityEngine;

public abstract class ValueBar<T> : MonoBehaviour
{
    ValueReference<T> currentValue;
    ValueReference<T> maxValue;

    [SerializeField]
    protected RectTransform bar;
    protected Vector2 barSize;

    // Start is called before the first frame update
    protected void Awake()
    {
        barSize = bar.sizeDelta;
        currentValue.SubscribeToChange(UpdateBar);
    }

    protected void SetReferences(ValueReference<T> currentValue, ValueReference<T> maxValue)
    {
        this.currentValue = currentValue;
        this.maxValue = maxValue;
    }

    public abstract void UpdateBar();
}
