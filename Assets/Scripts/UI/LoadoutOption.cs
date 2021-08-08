using UnityEngine;
using UnityEngine.UI;

public class LoadoutOption : MonoBehaviour
{
    public GameObject Ability;
    public bool isPassive = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!isPassive)
            GetComponentInChildren<Text>().text = Ability.GetComponent<Ability>().DisplayName;
        else
            GetComponentInChildren<Text>().text = Ability.gameObject.name;
        GetComponent<DragDrop>().OnInsert += OnInsert;
        GetComponent<DragDrop>().OnRemove += OnRemove;
        OnInsert(GetComponent<DragDrop>().assignedSlot);
    }

    void OnInsert(DropSlot dropSlot)
    {
        LoadoutSlot slot = dropSlot.GetComponent<LoadoutSlot>();
        slot.SetAbility(Ability, slot, true);
    }

    void OnRemove(DropSlot dropSlot)
    {
        LoadoutSlot slot = dropSlot.GetComponent<LoadoutSlot>();
        slot.SetAbility(Ability, slot, false);
    }
}
