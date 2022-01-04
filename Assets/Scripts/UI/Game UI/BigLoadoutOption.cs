using UnityEngine;
using UnityEngine.UI;

public class BigLoadoutOption : MonoBehaviour
{
    LoadoutSlot primarySlot;
    LoadoutSlot secondarySlot;

    // Start is called before the first frame update
    void Awake()
    {
        secondarySlot = GetComponentInChildren<LoadoutSlot>();
        secondarySlot.GetComponent<DropSlot>().OnInserted += OnSecondaryInsert;
        DragDrop dragDrop = GetComponent<DragDrop>();
        dragDrop.OnInsert += OnPrimaryInsert;
        dragDrop.OnRemove += OnPrimaryRemove;
    }

    private void Start()
    {
        //secondarySlot.GetComponent<DropSlot>().Offset = GetComponent<RectTransform>().anchoredPosition;
    }


    public void SetSecondaryAbility(string secondaryAbility)
    {
        secondarySlot.GetComponent<DropSlot>().Type = secondaryAbility;
        secondarySlot.GetComponentInChildren<Text>().text = secondaryAbility;
    }

    public void InsertOnSecondary(LoadoutOption option)
    {
        secondarySlot.GetComponent<DropSlot>().OnDrop(option.gameObject);
    }

    void OnSecondaryInsert(DragDrop option)
    {
        ((SecondaryAbility)option.GetComponent<LoadoutOption>().Ability).ParentAbility =
            GetComponent<LoadoutOption>().Ability.GetComponent<Ability>();
    }

    public void OnPrimaryInsert(DropSlot slot)
    {
        primarySlot = slot.GetComponent<LoadoutSlot>();

        // Inserting into the last slot
        if (primarySlot.NextSlot == null || primarySlot.NextSlot.GetComponent<DropSlot>().InsertedDragDrop != null)
            secondarySlot.gameObject.SetActive(false);

        // Inserting into an actual rational slot
        else
        {
            secondarySlot.gameObject.SetActive(true);
            secondarySlot.LoadoutNumber = primarySlot.LoadoutNumber;
            secondarySlot.AbilityNumber = primarySlot.AbilityNumber + 1;
            primarySlot.NextSlot.gameObject.SetActive(false);
        }

        secondarySlot.GetComponent<DropSlot>().Offset = GetComponent<RectTransform>().anchoredPosition;
        Debug.Log("Secondary Offset: " + secondarySlot.GetComponent<DropSlot>().Offset);
    }

    void OnPrimaryRemove(DropSlot slot)
    {
        DropSlot secondaryDropSlot = secondarySlot.GetComponent<DropSlot>();

        if (secondaryDropSlot.InsertedDragDrop && secondaryDropSlot.InsertedDragDrop.OnRemove != null)
            secondaryDropSlot.InsertedDragDrop.OnRemove?.Invoke(secondaryDropSlot);

        if (secondaryDropSlot.gameObject.activeInHierarchy)
        {
            primarySlot.NextSlot.gameObject.SetActive(true);
            if (secondaryDropSlot.InsertedDragDrop)
                secondaryDropSlot.OnRemove(secondaryDropSlot.InsertedDragDrop.gameObject);
            secondarySlot.gameObject.SetActive(false);
        }
    }
}
