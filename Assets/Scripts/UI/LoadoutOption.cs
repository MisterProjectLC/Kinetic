using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadoutOption : MonoBehaviour
{
    public GameObject Ability;
    public bool isPassive = false;
    public UnityAction<LoadoutOption> OnInsert;

    // Start is called before the first frame update
    void Start()
    {
        if (!isPassive)
        {
            GetComponentInChildren<Text>().text = Ability.GetComponent<Ability>().DisplayName;
            GetComponent<DragDrop>().Type = "Ability";
        }
        else
        {
            GetComponentInChildren<Text>().text = Ability.gameObject.name;
            GetComponent<DragDrop>().Type = "Passive";
        }

        if (Ability.GetComponent<Ability>() is SecondaryAbility)
            GetComponent<DragDrop>().Type = Ability.GetComponent<Ability>().DisplayName;

        GetComponent<DragDrop>().OnInsert += OnInsertFunc;
        GetComponent<DragDrop>().OnRemove += OnRemove;
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

    void OnRemove(DropSlot dropSlot)
    {
        LoadoutSlot slot = dropSlot.GetComponent<LoadoutSlot>();
        slot.SetAbility(Ability, slot, false);
    }
}
