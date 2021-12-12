using UnityEngine;

public class LoadoutSlot : MonoBehaviour
{
    public int AbilityNumber = 0;
    public int LoadoutNumber = 0;

    public LoadoutSlot NextSlot;

    public void SetAbility(GameObject ability, bool activating)
    {
        if (activating)
            Hermes.SpawnAbilities.Add(new Hermes.SavedAbility(ability.name, LoadoutNumber, AbilityNumber));
        else
            Hermes.SpawnAbilities.RemoveAll(x => x.loadout == LoadoutNumber && x.slot == AbilityNumber);

        if (LoadoutNumber >= 0)
            ActorsManager.Player.GetComponent<LoadoutManager>().SetAbility(activating ? ability : null, LoadoutNumber, AbilityNumber);
        else
            ActorsManager.Player.GetComponent<LoadoutManager>().SetPassive(ability, activating);
    }
}
