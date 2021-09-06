using UnityEngine;

public class BigLoadoutOption : MonoBehaviour
{
    LoadoutSlot secondarySlot;

    // Start is called before the first frame update
    void Start()
    {
        secondarySlot = GetComponentInChildren<LoadoutSlot>();
        DragDrop dragDrop = GetComponent<DragDrop>();
        dragDrop.OnInsert += OnInsert;
        dragDrop.OnRemove += OnRemove;
    }

    void OnInsert(DropSlot slot)
    {
        LoadoutSlot primarySlot = slot.GetComponent<LoadoutSlot>();
        if (primarySlot.AbilityNumber >= Constants.SlotsPerLoadout - 1)
            secondarySlot.gameObject.SetActive(false);
        else
        {
            secondarySlot.gameObject.SetActive(true);
            secondarySlot.LoadoutNumber = primarySlot.LoadoutNumber;
            secondarySlot.AbilityNumber = primarySlot.AbilityNumber + 1;
        }

        secondarySlot.GetComponent<DropSlot>().Offset = GetComponent<RectTransform>().anchoredPosition;
    }
    

    void OnRemove(DropSlot slot)
    {
        secondarySlot.gameObject.SetActive(false);
    }
}
