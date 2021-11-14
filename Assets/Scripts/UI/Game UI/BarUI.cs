using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUI : MonoBehaviour
{
    [SerializeField]
    RectTransform bar;
    Vector2 barSize;

    // Start is called before the first frame update
    void Awake()
    {
        barSize = bar.sizeDelta;
    }


    public void UpdateBar(float currentValue, float maxValue)
    {
        bar.sizeDelta = barSize * new Vector2(currentValue / maxValue, 1f);
    }
}
