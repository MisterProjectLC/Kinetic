using UnityEngine;
using UnityEngine.Events;

public class BigLoadoutOption : LoadoutOption
{
    AbilitySlot primarySlot;
    [SerializeField]
    SecondaryAbilitySlot secondarySlot;

    LocalizedString secondaryName = new LocalizedString();

    // Start is called before the first frame update
    protected void Awake()
    {
        secondarySlot.OnInsertedAbility += OnSecondaryInsert;
        SubscribeToInsert(OnPrimaryInsert);
        OnRemove += OnPrimaryRemove;
    }

    protected new void Start()
    {
        base.Start();
        //secondarySlot.GetComponent<DropSlot>().Offset = GetComponent<RectTransform>().anchoredPosition;
    }

    private void OnEnable()
    {
        UpdateSecondaryAbility();
    }

    public void SetupSecondaryAbility(ILoadoutManager loadoutManager, LocalizedString secondaryAbility)
    {
        secondaryName = secondaryAbility;
        secondarySlot.Setup(loadoutManager);
        UpdateSecondaryAbility();
    }

    void UpdateSecondaryAbility()
    {
        if (secondaryName.key == "")
            return;

        secondarySlot.SetSecondaryAbility(secondaryName);
    }

    public void InsertOnSecondary(LoadoutOption option)
    {
        secondarySlot.OnDrop(option.gameObject);
        Debug.Log("InsertOnSecondary");
    }

    void OnSecondaryInsert(LoadoutOption option)
    {
        ((SecondaryAbility)option.Option).ParentAbility = (Ability)Option;
    }

    public void OnPrimaryInsert(DropSlot slot)
    {
        primarySlot = (AbilitySlot)slot;

        // Inserting into the last slot
        if (primarySlot.NextSlot == null || primarySlot.NextSlot.InsertedDragDrop != null)
            secondarySlot.gameObject.SetActive(false);

        // Inserting into an actual rational slot
        else
        {
            secondarySlot.gameObject.SetActive(true);
            secondarySlot.LoadoutNumber = primarySlot.LoadoutNumber;
            secondarySlot.AbilityNumber = primarySlot.AbilityNumber + 1;
            primarySlot.NextSlot.gameObject.SetActive(false);
        }

        secondarySlot.Offset = GetComponent<RectTransform>().anchoredPosition;
        Debug.Log("Secondary Offset: " + secondarySlot.Offset);
    }

    void OnPrimaryRemove(DropSlot slot)
    {
        if (secondarySlot.InsertedDragDrop && secondarySlot.InsertedDragDrop.OnRemove != null)
            secondarySlot.InsertedDragDrop.OnRemove?.Invoke(secondarySlot);

        if (secondarySlot.gameObject.activeInHierarchy)
        {
            primarySlot.NextSlot.gameObject.SetActive(true);
            if (secondarySlot.InsertedDragDrop)
                secondarySlot.OnRemove(secondarySlot.InsertedDragDrop.gameObject);
            secondarySlot.gameObject.SetActive(false);
        }
    }
}
