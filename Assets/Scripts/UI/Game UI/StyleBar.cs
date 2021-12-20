using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleBar : BarUI
{
    StyleMeter styleMeter;

    [SerializeField]
    GameObject HypeBar;

    // Start is called before the first frame update
    void Start()
    {
        styleMeter = ActorsManager.Player.GetComponent<StyleMeter>();
        styleMeter.OnUpdate += UpdateStylebar;
        styleMeter.OnCritical += UpdateCritical;
        
    }


    void UpdateStylebar()
    {
        UpdateBar(styleMeter.JuiceLeft, styleMeter.JuiceMax);
    }

    void UpdateCritical(bool critical)
    {
        HypeBar.SetActive(critical);
    }
}
