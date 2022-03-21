using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public static LevelUpSystem LUS;

    public enum Type
    {
        _First,
        Passive,
        Skill,
        Weapon,
        _Last
    }

    // Loadouts
    [System.Serializable]
    public struct Loadout
    {
        public List<DropSlot> slots;
    }

    [SerializeField]
    DropSlot globalSlot;
    [SerializeField]
    List<Loadout> totalSlots;
    [SerializeField]
    List<DropSlot> passiveSlots;
    [SerializeField]
    List<DropSlot> initialSlots;
    [SerializeField]
    GameObject NewAbilityText;
    [SerializeField]
    GameObject NewAbilitySquare;
    [SerializeField]
    CanvasGroup DragInstructions;

    [Header("Prefab References")]
    public GameObject optionPrefab;
    public GameObject heavyOptionPrefab;

    List<LoadoutOption> optionsLocked = new List<LoadoutOption>();
    Dictionary<Type, List<LoadoutOption>> optionsBank =  new Dictionary<Type, List<LoadoutOption>>();
    List<LoadoutOption> optionsShown = new List<LoadoutOption>();
    List<LoadoutOption> optionsObtained = new List<LoadoutOption>();

    bool loweredMenu = false;
    int siblingBaseCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (LUS)
            Destroy(LUS);

        LUS = this;

        for (Type i = Type._First+1; i < Type._Last; i++)
            optionsBank.Add(i, new List<LoadoutOption>());

        for (int i = 0; i < totalSlots.Count; i++)
            totalSlots[i].slots.Add(globalSlot);
    }


    void AddToOptionsBank(LoadoutOption loadoutOption)
    {
        if (!loadoutOption.Ability)
            optionsBank[Type.Passive].Add(loadoutOption);
        else if (loadoutOption.Ability is WeaponAbility)
            optionsBank[Type.Weapon].Add(loadoutOption);
        else
            optionsBank[Type.Skill].Add(loadoutOption);
    }


    void Start() {

        LoadoutManager loadout = ActorsManager.Player.GetComponent<LoadoutManager>();
        siblingBaseCount = transform.GetChildCount();

        int i = 0;
        // Populate initial options
        foreach (LoadoutManager.Option option in loadout.InitialOptions)
        {
            LoadoutOption loadoutOption = GenerateOptionInstance(option);

            if (option.ability is SecondaryAbility)
            {
                i--;
                //initialSlots[i].OnDrop(loadoutOption.gameObject);
                initialSlots[i-1].InsertedDragDrop.GetComponent<BigLoadoutOption>().InsertOnSecondary(loadoutOption);
            }
            else
            {
                initialSlots[i].OnDrop(loadoutOption.gameObject);
                if (i != 0 && option.secondaryAbility.value.Length > 0)
                    i++;
            }
            i++;
        }

        // Prepare saved options
        for (int j = 0; j < Hermes.SpawnAbilities.Count; j++)
            Hermes.SpawnAbilities[j].SetInGame(false);
                       

        // Populate options
        foreach (LoadoutManager.Option option in loadout.Options)
        {
            LoadoutOption loadoutOption = GenerateOptionInstance(option);
            GameObject GO = loadoutOption.gameObject;

            // Makeshift code for initial Killheal passive
            if (option.option.name == "Killheal")
            {
                passiveSlots[0].OnDrop(GO);
                optionsObtained.Add(loadoutOption);
            }
            // Setup option
            else
            {
                // Check if already saved
                if (Hermes.SpawnAbilities.Exists(x => x.name == option.option.name))
                {
                    Hermes.SavedAbility ability = Hermes.SpawnAbilities.Find(x => x.name == option.option.name);
                    Hermes.SpawnAbilities[Hermes.SpawnAbilities.IndexOf(ability)].SetInGame(true);

                    if (ability.loadout != -1)
                        totalSlots[ability.loadout].slots[ability.slot].OnDrop(GO);
                    else
                        passiveSlots[ability.slot].OnDrop(GO);
                    optionsObtained.Add(loadoutOption);
                }
                else
                {
                    GO.SetActive(false);
                    if (option.prerequisiteAbilities.Count > 0)
                        optionsLocked.Add(loadoutOption);
                    else
                        AddToOptionsBank(loadoutOption);
                }
            }
        }

        // Fix for secondary abilities
        foreach (Loadout thisLoadout in totalSlots)
            foreach (DropSlot slot in thisLoadout.slots)
            {
                // Check if there's a big loadout option here
                DragDrop dragDrop = slot.InsertedDragDrop;
                if (!dragDrop)
                    continue;
                BigLoadoutOption bigOption = dragDrop.GetComponent<BigLoadoutOption>();
                if (!bigOption)
                    continue;

                // Check, find and insert its secondary ability
                int nextSlotIndex = slot.GetComponent<LoadoutSlot>().AbilityNumber + 1;
                if (nextSlotIndex >= thisLoadout.slots.Count || !thisLoadout.slots[nextSlotIndex].InsertedDragDrop)
                    continue;

                //bigOption.InsertOnSecondary(thisLoadout.slots[nextSlotIndex].InsertedDragDrop.GetComponent<LoadoutOption>());
            }
        
    }

    LoadoutOption GenerateOptionInstance(LoadoutManager.Option option)
    {
        // Instantiate object
        GameObject newInstance = null;
        if (option.secondaryAbility.value.Length <= 0)
            newInstance = Instantiate(optionPrefab);
        else
        {
            newInstance = Instantiate(heavyOptionPrefab);
            newInstance.GetComponent<BigLoadoutOption>().SetSecondaryAbility(option.secondaryAbility);
        }

        // Set sibling order
        newInstance.transform.SetParent(transform);
        if (option.secondaryAbility.value.Length > 0)
            newInstance.transform.SetSiblingIndex(siblingBaseCount);

        // Setup loadout option
        LoadoutOption loadoutOption = newInstance.GetComponent<LoadoutOption>();
        loadoutOption.Option = option.option;
        loadoutOption.PrerequisiteAbilities = new List<string>(option.prerequisiteAbilities);
        loadoutOption.OnInsert += ChooseOption;
        loadoutOption.GetComponent<DragDrop>().OnClick += () => StartCoroutine(InstructionsAnimation());

        return loadoutOption;
    }


    public void LevelUp(Type type)
    {
        List<LoadoutOption> thisOptionsBank = optionsBank[type];
        NewAbilityText.SetActive(true);
        NewAbilitySquare.SetActive(true);
        SetLoweredMenu(false);

        for (int i = 0; i < 3 && thisOptionsBank.Count > 0; i++) {
            int rnd = Random.Range(0, thisOptionsBank.Count);

            thisOptionsBank[rnd].gameObject.SetActive(true);
            optionsShown.Add(optionsBank[type][rnd]);
            thisOptionsBank[rnd].GetComponent<RectTransform>().anchoredPosition = new Vector2(-40 + i*258, 166);
            if (thisOptionsBank[rnd].GetComponent<BigLoadoutOption>())
                SetLoweredMenu(true);

            thisOptionsBank.RemoveAt(rnd);
        }
    }

    public void DebugLevelUp()
    {
        LevelUp(Type.Passive);
    }

    void SetLoweredMenu(bool loweredMenu)
    {
        if (this.loweredMenu == loweredMenu)
            return;

        this.loweredMenu = loweredMenu;
        foreach (RectTransform rectTransform in GetComponentsInChildren<RectTransform>())
        {
            if (rectTransform.gameObject.name == "LM-Background")
                rectTransform.anchoredPosition += loweredMenu ? -new Vector2(0f, 60f) : new Vector2(0f, 60f);
        }
    }


    void ChooseOption(LoadoutOption option)
    {
        if (!optionsShown.Contains(option))
            return;

        // Add options
        optionsShown.Remove(option);
        optionsObtained.Add(option);

        // Unlock locked loadouts
        List<LoadoutOption> optionsToDelete = new List<LoadoutOption>();
        foreach (LoadoutOption potentialToUnlock in optionsLocked)
        {
            string displayName = option.Ability ? option.Ability.LocalizedName.value : option.Option.name;
            if (potentialToUnlock.PrerequisiteAbilities.Contains(displayName))
            {
                potentialToUnlock.PrerequisiteAbilities.Remove(displayName);
                if (potentialToUnlock.PrerequisiteAbilities.Count <= 0)
                {
                    optionsToDelete.Add(potentialToUnlock);
                    AddToOptionsBank(potentialToUnlock);
                }
            }
        }
        foreach (LoadoutOption optionToDelete in optionsToDelete)
            optionsLocked.Remove(optionToDelete);


        // Hide shown options
        foreach (LoadoutOption shownOption in optionsShown)
        {
            shownOption.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, 1000);
            shownOption.gameObject.SetActive(false);
            AddToOptionsBank(shownOption);
        }
        optionsShown.Clear();
        NewAbilityText.SetActive(false);
        NewAbilitySquare.SetActive(false);
    }


    IEnumerator InstructionsAnimation()
    {
        for (int i = 0; i < 6; i++)
        {
            Debug.Log(DragInstructions.alpha);
            yield return new WaitForSecondsRealtime(0.1f);
            DragInstructions.alpha = DragInstructions.alpha == 0 ? 1 : 0;
        }
    }

    public int GetLoadoutCount()
    {
        return totalSlots.Count;
    }
}
