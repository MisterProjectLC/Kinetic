using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutMenu : Menu
{
    ILoadoutManager loadoutManager;

    [SerializeField]
    Text LoadoutText;

    StringBuilder stringBuilder = new StringBuilder(15);

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
            LoadoutText.text = stringBuilder.Clear().Append(Loadout.value).Append(" ").Append(1+loadoutManager.GetCurrentLoadoutIndex()).ToString();
            SubmenuButton(loadoutManager.GetCurrentLoadoutIndex());
        }
    }

    public override void SubmenuButton(int i)
    {
        base.SubmenuButton(i);
        LoadoutText.text = stringBuilder.Clear().Append(Loadout.value).Append(" ").Append(1 + i).ToString();
    }
}
