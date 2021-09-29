using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowtimeBar : BarUI
{
    SlowtimePower slowtime;

    // Start is called before the first frame update
    void Start()
    {
        slowtime = ActorsManager.Player.GetComponent<SlowtimePower>();
        slowtime.OnUpdate += UpdateSlowtimebar;
    }


    void UpdateSlowtimebar()
    {
        UpdateBar(slowtime.JuiceLeft, slowtime.JuiceMax);
    }


}
