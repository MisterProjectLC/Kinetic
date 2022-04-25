using UnityEngine;
using UnityEngine.Events;

public class AbilitySlot : LoadoutSlot
{
    public int LoadoutNumber = 0;
    public bool GlobalSlot = false;

    internal UnityAction<LoadoutOption> OnInsertedAbility;

    public LoadoutSlot NextSlot;

    private void Start()
    {
        Type = "Ability";
        OnInsertedAbility += (LoadoutOption a) => { OnInserted(a); };
    }

    public override void SetOptionPrivate(Upgrade option, bool activating)
    {
        if (GlobalSlot)
            for (int i = 0; i < LevelUpSystem.LUS.GetLoadoutCount(); i++)
                loadoutManager.SetAbility(activating ? option.GetComponent<Ability>() : null, i, AbilityNumber);
        else
        {
            if (LoadoutNumber >= 0)
                loadoutManager.SetAbility(activating ? option.GetComponent<Ability>() : null, LoadoutNumber, AbilityNumber);
            else
                loadoutManager.SetPassive(option, activating);
        }
    }

    protected override int GetLoadoutNumber()
    {
        return LoadoutNumber;
    }
}
