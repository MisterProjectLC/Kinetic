using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadoutOption : MonoBehaviour
{
    GameObject option;
    public GameObject Option
    {
        get { return option; }
        set
        {
            option = value;
            Ability = option.GetComponent<Ability>();
        }
    }
    public Ability Ability { get; private set; }
    public List<string> PrerequisiteAbilities;

    [SerializeField]
    Image passiveOverlay;
    public UnityAction<LoadoutOption> OnInsert;

    TooltipTrigger tooltipTrigger;
    DragDrop dragDrop;

    LocalizedString localizedString = new LocalizedString("");
    Text label;


    // Start is called before the first frame update
    void Start()
    {
        string abilityName;
        dragDrop = GetComponent<DragDrop>();

        if (Ability)
        {
            localizedString = Ability.LocalizedName;
            abilityName = localizedString.value;
            dragDrop.Type = Ability is SecondaryAbility ? localizedString.value : "Ability";
        } else 
        {
            GetComponentInChildren<Text>().text = Option.name;
            abilityName = Option.name;
            dragDrop.Type = "Passive";
            passiveOverlay.enabled = true;
        }

        label = GetComponentInChildren<Text>();
        label.text = abilityName;

        tooltipTrigger = GetComponent<TooltipTrigger>();
        tooltipTrigger.Title = abilityName;

        dragDrop.OnInsert += OnInsertFunc;
        dragDrop.OnRemove += RemoveFromSlot;
        if (dragDrop.AssignedSlot)
            OnInsertFunc(dragDrop.AssignedSlot);

        UpdateText();
    }


    private void OnEnable()
    {
        UpdateText();
    }

    void OnInsertFunc(DropSlot dropSlot)
    {
        LoadoutSlot slot = dropSlot.GetComponent<LoadoutSlot>();
        slot.SetOption(Option, true);
        OnInsert?.Invoke(this);
    }

    void RemoveFromSlot(DropSlot dropSlot)
    {
        LoadoutSlot slot = dropSlot.GetComponent<LoadoutSlot>();
        slot.SetOption(Option, false);
    }


    void UpdateText()
    {
        if (!Option || localizedString.key == "")
            return;

        if (Ability)
        {
            if (Ability is SecondaryAbility)
                dragDrop.Type = localizedString.value;

            label.text = localizedString.value;
            tooltipTrigger.Description = "COOLDOWN: " + Ability.Cooldown.ToString() +
                " " + LocalizationSystem.GetLocalizedText("seconds") + "\n" + Ability.GetComponent<Description>().Value;
        }
        else
            tooltipTrigger.Description = Option.GetComponent<Description>().Value;
    }
}
