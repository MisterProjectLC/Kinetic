using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutMenu : Menu
{
    ILoadoutManager loadoutManager;

    new void Start()
    {
        
        loadoutManager = GetComponent<ILoadoutManager>();
        base.Start();
    }

    private void OnEnable()
    {
        if (loadoutManager != null && !submenus[loadoutManager.GetCurrentLoadoutIndex()].enabled)
            SubmenuButton(loadoutManager.GetCurrentLoadoutIndex());
    }
}
