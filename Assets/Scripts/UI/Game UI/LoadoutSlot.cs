using UnityEngine;

public class LoadoutSlot : MonoBehaviour
{
    public int AbilityNumber = 0;
    public int LoadoutNumber = 0;

    public LoadoutSlot NextSlot;

    public void SetOption(GameObject option, bool activating)
    {
        if (activating)
            Hermes.SpawnAbilities.Add(new Hermes.SavedAbility(option.name, LoadoutNumber, AbilityNumber));
        else
            Hermes.SpawnAbilities.RemoveAll(x => x.loadout == LoadoutNumber && x.slot == AbilityNumber);

        if (LoadoutNumber >= 0)
            ActorsManager.Player.GetComponent<LoadoutManager>().SetAbility(activating ? option.GetComponent<Ability>() : null, 
                LoadoutNumber, AbilityNumber);
        else
            ActorsManager.Player.GetComponent<LoadoutManager>().SetPassive(option, activating);
    }
}
