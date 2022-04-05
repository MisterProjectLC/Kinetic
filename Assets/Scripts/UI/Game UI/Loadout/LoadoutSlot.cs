using UnityEngine;

public class LoadoutSlot : MonoBehaviour
{
    public int AbilityNumber = 0;
    public int LoadoutNumber = 0;
    public bool GlobalSlot = false;

    public LoadoutSlot NextSlot;
    ILoadoutManager loadoutManager;

    public void Setup(ILoadoutManager loadoutManager)
    {
        this.loadoutManager = loadoutManager;
    }

    public void SetOption(GameObject option, bool activating)
    {
        if (activating)
            Hermes.SpawnAbilities.Add(new Hermes.SavedAbility(option.name, LoadoutNumber, AbilityNumber));
        else
            Hermes.SpawnAbilities.RemoveAll(x => x.loadout == LoadoutNumber && x.slot == AbilityNumber);


        if (GlobalSlot)
            for (int i = 0; i < LevelUpSystem.LUS.GetLoadoutCount(); i++)
                loadoutManager.SetAbility(activating ? option.GetComponent<Ability>() : null, i, AbilityNumber);
        else
            if (LoadoutNumber >= 0)
                loadoutManager.SetAbility(activating ? option.GetComponent<Ability>() : null, LoadoutNumber, AbilityNumber);
            else
                loadoutManager.SetPassive(option, activating);
    }
}
