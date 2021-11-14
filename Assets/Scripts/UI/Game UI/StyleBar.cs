using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleBar : BarUI
{
    StyleMeter styleMeter;

    // Start is called before the first frame update
    void Start()
    {
        styleMeter = ActorsManager.Player.GetComponent<StyleMeter>();
        styleMeter.OnUpdate += UpdateStylebar;
    }


    void UpdateStylebar()
    {
        UpdateBar(styleMeter.JuiceLeft, styleMeter.JuiceMax);
    }


}
