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


    // Start is called before the first frame update
    void Start()
    {
        string abilityName;

        if (Ability)
        {
            abilityName = Ability.DisplayName;
            GetComponent<DragDrop>().Type = "Ability";
        } else 
        {
            GetComponentInChildren<Text>().text = Option.name;
            abilityName = Option.name;
            GetComponent<DragDrop>().Type = "Passive";
            passiveOverlay.enabled = true;
        }

        GetComponentInChildren<Text>().text = abilityName;
        GetComponent<TooltipTrigger>().Title = abilityName;


        if (Ability && Ability is SecondaryAbility)
            GetComponent<DragDrop>().Type = Ability.DisplayName;

        GetComponent<DragDrop>().OnInsert += OnInsertFunc;
        GetComponent<DragDrop>().OnRemove += RemoveFromSlot;
        if (GetComponent<DragDrop>().AssignedSlot)
            OnInsertFunc(GetComponent<DragDrop>().AssignedSlot);
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
        if (!Option)
            return;

        if (Ability)
            GetComponent<TooltipTrigger>().Description = "COOLDOWN: " + Ability.Cooldown.ToString() +
                " " + LocalizationSystem.GetLocalizedText("seconds") + "\n" + Ability.GetComponent<Description>().Value;
        else
            GetComponent<TooltipTrigger>().Description = Option.GetComponent<Description>().Value;
    }
}
