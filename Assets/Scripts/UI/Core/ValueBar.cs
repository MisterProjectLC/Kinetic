using UnityEngine;
using SmartData.Abstract;

public class ValueBar : MonoBehaviour
{
    [SerializeField]
    protected RectTransform bar;
    protected Vector2 barSize;

    // Start is called before the first frame update
    void Awake()
    {
        barSize = bar.sizeDelta;
    }
}
