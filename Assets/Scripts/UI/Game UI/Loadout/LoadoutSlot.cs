using UnityEngine;

public abstract class LoadoutSlot : DropSlot
{
    public int AbilityNumber = 0;

    [HideInInspector]
    public ILoadoutManager loadoutManager;


    new void Awake()
    {
        OnInserted += OnInsertedOption;
        OnRemoved += OnRemovedOption;
        base.Awake();
    }

    public void Setup(ILoadoutManager loadoutManager)
    {
        this.loadoutManager = loadoutManager;
    }

    protected abstract int GetLoadoutNumber();

    public void OnInsertedOption(DragDrop dragDrop)
    {
        Debug.Log("OnInsertedOption");
        Upgrade option = ((LoadoutOption)dragDrop).Option;
        SavedLoadout.AddSpawnAbility(new SavedLoadout.SavedAbility(option.name, GetLoadoutNumber(), AbilityNumber));
        SetOptionPrivate(option, true);
    }

    public void OnRemovedOption(DragDrop dragDrop)
    {
        Upgrade option = ((LoadoutOption)dragDrop).Option;
        SavedLoadout.RemoveSpawnAbility(GetLoadoutNumber(), AbilityNumber);
        SetOptionPrivate(option, false);
    }

    public abstract void SetOptionPrivate(Upgrade option, bool activating);
}
