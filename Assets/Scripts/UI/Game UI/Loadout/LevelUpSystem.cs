using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelUpSystem : MonoBehaviour
{
    public static LevelUpSystem LUS;

    public enum Type
    {
        _First,
        Passive,
        Ability,
        Weapon,
        _Last
    }

    // Loadouts
    [System.Serializable]
    public struct Loadout
    {
        public List<LoadoutSlot> slots;
    }

    [SerializeField]
    LoadoutSlot globalSlot;
    [SerializeField]
    List<Loadout> abilitySlots;
    [SerializeField]
    List<LoadoutSlot> passiveSlots;
    [SerializeField]
    List<LoadoutSlot> setupSlots;
    [SerializeField]
    GameObject NewAbilityText;
    [SerializeField]
    GameObject NewAbilitySquare;
    [SerializeField]
    CanvasGroup DragInstructions;

    [Header("Prefab References")]
    [SerializeField]
    GameObject optionPrefab;
    [SerializeField]
    GameObject heavyOptionPrefab;

    List<LoadoutOption> optionsLocked = new List<LoadoutOption>();
    Dictionary<Type, List<LoadoutOption>> optionsBank =  new Dictionary<Type, List<LoadoutOption>>();
    List<LoadoutOption> optionsShown = new List<LoadoutOption>();
    List<LoadoutOption> optionsObtained = new List<LoadoutOption>();

    ILoadoutManager loadoutManager;

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

        for (int i = 0; i < abilitySlots.Count; i++)
            abilitySlots[i].slots.Add(globalSlot);

        siblingBaseCount = transform.childCount;

        loadoutManager = GetComponent<ILoadoutManager>();
        SetupLoadoutSlots(loadoutManager);
    }


    void AddToOptionsBank(LoadoutOption loadoutOption)
    {
        if (loadoutOption.Option is Passive)
            optionsBank[Type.Passive].Add(loadoutOption);
        else if (loadoutOption.Option is WeaponAbility)
            optionsBank[Type.Weapon].Add(loadoutOption);
        else
            optionsBank[Type.Ability].Add(loadoutOption);
    }


    void Start() 
    {
        loadoutManager.SubscribeToNewUpgrade(OnNewUpgrade);

        // Prepare saved options
        for (int j = 0; j < SavedLoadout.GetSpawnAbilityCount(); j++)
            SavedLoadout.GetSpawnAbility(j).SetInGame(false);

        PopulateOptions(loadoutManager.GetInitialPassives(), HandleInitialPassive);
        PopulateOptions(loadoutManager.GetInitialOptions(), HandleInitialOption);
        PopulateOptions(loadoutManager.GetOptions(), HandleOption);
    }

    void SetupLoadoutSlots(ILoadoutManager loadoutManager)
    {
        foreach (Loadout loadout in abilitySlots)
        {
            foreach (AbilitySlot slot in loadout.slots)
            {
                slot.Setup(loadoutManager);
            }
        }
        foreach (PassiveSlot slot in passiveSlots)
        {
            slot.Setup(loadoutManager);
        }
    }

    void PopulateOptions(List<ILoadoutManager.Option> optionsList, Action<LoadoutOption> optionHandler)
    {
        // Populate options
        foreach (ILoadoutManager.Option option in optionsList)
        {
            LoadoutOption loadoutOption = GenerateOptionInstance(option);
            optionHandler.Invoke(loadoutOption);
        }
    }


    void HandleInitialPassive(LoadoutOption loadoutOption)
    {
        InsertUpgrade(-1, 0, loadoutOption);
    }

    void HandleInitialOption(LoadoutOption loadoutOption)
    {
        // Check if already saved
        if (SavedLoadout.ExistsSpawnAbility(loadoutOption.Option.name))
        {
            SavedLoadout.SavedAbility upgrade = SavedLoadout.GetSpawnAbility(loadoutOption.Option.name);
            SavedLoadout.SetSpawnAbility(upgrade, SavedLoadout.GetSpawnAbilityIndex(upgrade));

            InsertUpgrade(upgrade.loadout, upgrade.slot, loadoutOption);
        }
        else
        {
            loadoutOption.gameObject.SetActive(false);
        }
    }

    void HandleOption(LoadoutOption loadoutOption)
    {
        // Check if already saved
        if (SavedLoadout.ExistsSpawnAbility(loadoutOption.Option.name))
        {
            SavedLoadout.SavedAbility upgrade = SavedLoadout.GetSpawnAbility(loadoutOption.Option.name);
            SavedLoadout.SetSpawnAbility(upgrade, SavedLoadout.GetSpawnAbilityIndex(upgrade));

            InsertUpgrade(upgrade.loadout, upgrade.slot, loadoutOption);
        }
        else
        {
            loadoutOption.gameObject.SetActive(false);
            if (loadoutOption.PrerequisiteAbilities.Count > 0)
                optionsLocked.Add(loadoutOption);
            else
                AddToOptionsBank(loadoutOption);
        }
    }


    void InsertUpgrade(int loadout, int slot, LoadoutOption loadoutOption)
    {
        if (loadout != -1)
        {
            if (loadoutOption.Option is SecondaryAbility)
                ((BigLoadoutOption)abilitySlots[loadout].slots[slot - 1].InsertedDragDrop).InsertOnSecondary(loadoutOption);
            else
                abilitySlots[loadout].slots[slot].OnDrop(loadoutOption.gameObject);

        }
        else
            passiveSlots[slot].OnDrop(loadoutOption.gameObject);
        optionsObtained.Add(loadoutOption);
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
            newInstance.GetComponent<BigLoadoutOption>().SetupSecondaryAbility(loadoutManager, option.secondaryAbility);
        }

        // Set sibling order
        newInstance.transform.SetParent(transform);
        if (option.secondaryAbility.value.Length > 0)
            newInstance.transform.SetSiblingIndex(siblingBaseCount);

        // Setup loadout option
        LoadoutOption loadoutOption = newInstance.GetComponent<LoadoutOption>();
        loadoutOption.Option = option.option;
        loadoutOption.PrerequisiteAbilities = new List<LocalizedString>(option.prerequisiteAbilities);
        loadoutOption.SubscribeToInsertSelf(ChooseOption);
        loadoutOption.OnClick += () => StartCoroutine(InstructionsAnimation());

        return loadoutOption;
    }


    public void LevelUp(Type type)
    {
        List<LoadoutOption> thisOptionsBank = optionsBank[type];
        NewAbilityText.SetActive(true);
        NewAbilitySquare.SetActive(true);
        SetLoweredMenu(false);

        for (int i = 0; i < 3 && thisOptionsBank.Count > 0; i++) {
            int rnd = UnityEngine.Random.Range(0, thisOptionsBank.Count);

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
            string displayName = option.Option.LocalizedName.value;
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


    void OnNewUpgrade(int loadout, int slot, Upgrade upgrade)
    {
        if (loadoutManager.GetInitialOptions().Exists(option => option.option == upgrade))
        {
            ILoadoutManager.Option option = loadoutManager.GetInitialOptions().Find(option => option.option == upgrade);
            LoadoutOption loadoutOption = GenerateOptionInstance(option);
            InsertUpgrade(loadout, slot, loadoutOption);

            SavedLoadout.SavedAbility savedAbility = SavedLoadout.GetSpawnAbility(option.option.name);
            SavedLoadout.SetSpawnAbility(savedAbility, SavedLoadout.GetSpawnAbilityIndex(savedAbility));
        }
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
        return abilitySlots.Count;
    }
}
