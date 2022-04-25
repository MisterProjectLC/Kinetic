using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutMenu : Menu
{
    ILoadoutManager loadoutManager;

    [SerializeField]
    Text LoadoutText;

    [SerializeField]
    LocalizedString Loadout;

    new void Start()
    {
        
        loadoutManager = GetComponent<ILoadoutManager>();
        base.Start();
    }

    private void OnEnable()
    {
        if (loadoutManager != null && !submenus[loadoutManager.GetCurrentLoadoutIndex()].enabled)
        {
            LoadoutText.text = Loadout.value + " " + (1+loadoutManager.GetCurrentLoadoutIndex()).ToString();
            SubmenuButton(loadoutManager.GetCurrentLoadoutIndex());
        }
    }
}
