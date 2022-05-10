using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadoutOption : DragDrop
{
    public Upgrade Option;
    public List<LocalizedString> PrerequisiteAbilities;

    [SerializeField]
    Image passiveOverlay;
    protected UnityAction<LoadoutOption> OnInsertSelf;
    public void SubscribeToInsertSelf(UnityAction<LoadoutOption> subscriber) { OnInsertSelf += subscriber; }

    TooltipTrigger tooltipTrigger;

    LocalizedString localizedString = new LocalizedString("");
    Text label;


    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        localizedString = Option.LocalizedName;
        string abilityName = localizedString.value;
        Type = Option.Type;

        if (Option is Passive)
            passiveOverlay.enabled = true;

        label = GetComponentInChildren<Text>();
        label.text = abilityName;

        tooltipTrigger = GetComponent<TooltipTrigger>();
        tooltipTrigger.Title = abilityName;

        SubscribeToInsert(OnInsertFunc);
        if (AssignedSlot)
            OnInsertFunc(AssignedSlot);

        UpdateText();
    }


    private void OnEnable()
    {
        UpdateText();
    }

    void OnInsertFunc(DropSlot dropSlot)
    {
        OnInsertSelf?.Invoke(this);
    }


    void UpdateText()
    {
        if (!Option || localizedString.key == "")
            return;

        label.text = localizedString.value;
        if (Option is SecondaryAbility)
            Type = localizedString.value;

        string description = Option.GetComponent<Description>().Value;
        tooltipTrigger.Description = Option is Ability ? "COOLDOWN: " + ((Ability)Option).Cooldown.ToString() +
                " " + LocalizationSystem.GetLocalizedText("seconds") + "\n" + description : description;
    }
}
