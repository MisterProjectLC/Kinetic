using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadoutOption : MonoBehaviour
{
    public GameObject Ability;
    public bool isPassive = false;
    [SerializeField]
    Image passiveOverlay;
    public UnityAction<LoadoutOption> OnInsert;

    // Start is called before the first frame update
    void Start()
    {
        string abilityName;

        if (!isPassive)
        {
            abilityName = Ability.GetComponent<Ability>().DisplayName;
            GetComponent<TooltipTrigger>().Description = "COOLDOWN: " + Ability.GetComponent<Ability>().Cooldown.ToString() + 
                " seconds\n" + Ability.GetComponent<Description>().description;
            GetComponent<DragDrop>().Type = "Ability";
        } else 
        {
            GetComponentInChildren<Text>().text = Ability.gameObject.name;
            abilityName = Ability.gameObject.name;
            GetComponent<TooltipTrigger>().Description = Ability.GetComponent<Description>().description;
            GetComponent<DragDrop>().Type = "Passive";
            passiveOverlay.enabled = true;
        }

        GetComponentInChildren<Text>().text = abilityName;
        GetComponent<TooltipTrigger>().Title = abilityName;


        if (Ability.GetComponent<Ability>() is SecondaryAbility)
            GetComponent<DragDrop>().Type = Ability.GetComponent<Ability>().DisplayName;

        GetComponent<DragDrop>().OnInsert += OnInsertFunc;
        GetComponent<DragDrop>().OnRemove += RemoveFromSlot;
        if (GetComponent<DragDrop>().AssignedSlot)
            OnInsertFunc(GetComponent<DragDrop>().AssignedSlot);
    }

    void OnInsertFunc(DropSlot dropSlot)
    {
        LoadoutSlot slot = dropSlot.GetComponent<LoadoutSlot>();
        slot.SetAbility(Ability, slot, true);
        if (OnInsert != null)
            OnInsert.Invoke(this);
    }

    void RemoveFromSlot(DropSlot dropSlot)
    {
        LoadoutSlot slot = dropSlot.GetComponent<LoadoutSlot>();
        slot.SetAbility(Ability, slot, false);
    }
}
