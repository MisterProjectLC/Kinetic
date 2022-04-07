using UnityEngine;

public abstract class LoadoutSlot : DropSlot
{
    public int AbilityNumber = 0;

    [HideInInspector]
    public ILoadoutManager loadoutManager;

    public void Setup(ILoadoutManager loadoutManager)
    {
        this.loadoutManager = loadoutManager;
    }

    protected abstract int GetLoadoutNumber();

    public void SetOption(Upgrade option, bool activating)
    {
        if (activating)
            SavedLoadout.AddSpawnAbility(new SavedLoadout.SavedAbility(option.name, GetLoadoutNumber(), AbilityNumber));
        else
            SavedLoadout.RemoveSpawnAbility(GetLoadoutNumber(), AbilityNumber);

        SetOptionPrivate(option, activating);
    }

    public abstract void SetOptionPrivate(Upgrade option, bool activating);
}
