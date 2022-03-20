using UnityEngine;

public class LoadoutSlot : MonoBehaviour
{
    public int AbilityNumber = 0;
    public int LoadoutNumber = 0;
    public bool GlobalSlot = false;

    public LoadoutSlot NextSlot;

    public void SetOption(GameObject option, bool activating)
    {
        if (activating)
            Hermes.SpawnAbilities.Add(new Hermes.SavedAbility(option.name, LoadoutNumber, AbilityNumber));
        else
            Hermes.SpawnAbilities.RemoveAll(x => x.loadout == LoadoutNumber && x.slot == AbilityNumber);


        if (GlobalSlot)
            for (int i = 0; i < LevelUpSystem.LUS.GetLoadoutCount(); i++)
                ActorsManager.Player.GetComponent<LoadoutManager>().SetAbility(activating ? option.GetComponent<Ability>() : null,
                    i, AbilityNumber);
        else

            if (LoadoutNumber >= 0)
                ActorsManager.Player.GetComponent<LoadoutManager>().SetAbility(activating ? option.GetComponent<Ability>() : null, 
                    LoadoutNumber, AbilityNumber);
            else
                ActorsManager.Player.GetComponent<LoadoutManager>().SetPassive(option, activating);
    }
}
