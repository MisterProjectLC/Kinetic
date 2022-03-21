using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutMenu : Menu
{
    LoadoutManager loadoutManager;

    new void Start()
    {
        
        loadoutManager = ActorsManager.Player.GetComponent<LoadoutManager>();
        base.Start();
    }

    private void OnEnable()
    {
        if (loadoutManager && !submenus[loadoutManager.currentLoadout].enabled)
            SubmenuButton(loadoutManager.currentLoadout);
    }
}
