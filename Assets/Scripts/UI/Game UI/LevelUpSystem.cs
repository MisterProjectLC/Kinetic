using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public static LevelUpSystem LUS;

    // Loadouts
    [System.Serializable]
    public struct Loadout
    {
        public List<RectTransform> slots;
    }

    [SerializeField]
    List<Loadout> totalSlots;
    [SerializeField]
    List<RectTransform> initialSlots;
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
    List<LoadoutOption> optionsBank = new List<LoadoutOption>();
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
    }

    void Start() {

        LoadoutManager loadout = ActorsManager.Player.GetComponent<LoadoutManager>();
        siblingBaseCount = transform.GetChildCount();

        int i = 0;
        // Populate initial options
        foreach (LoadoutManager.Option option in loadout.InitialOptions)
        {
            LoadoutOption loadoutOption = GenerateOptionInstance(option).GetComponent<LoadoutOption>();
            initialSlots[i].GetComponent<DropSlot>().OnDrop(loadoutOption.gameObject);
            if (i != 0 && option.secondaryAbility.Length > 0)
                i++;
            i++;
        }

        // Prepare saved options
        for (int j = 0; j < Hermes.SpawnAbilities.Count; j++)
            Hermes.SpawnAbilities[i].SetInGame(false);
                       

        // Populate options
        foreach (LoadoutManager.Option option in loadout.Options)
        {
            GameObject GO = GenerateOptionInstance(option);

            // Makeshift code for initial Killheal passive
            if (option.ability.name == "Killheal")
            {
                totalSlots[3].slots[0].GetComponent<DropSlot>().OnDrop(GO.GetComponent<LoadoutOption>().gameObject);
                optionsObtained.Add(GO.GetComponent<LoadoutOption>());
            }
            // Setup option
            else
            {
                LoadoutOption loadoutOption = GO.GetComponent<LoadoutOption>();

                // Check if already saved
                if (Hermes.SpawnAbilities.Exists(x => x.name == option.ability.name))
                {
                    Hermes.SavedAbility ability = Hermes.SpawnAbilities.Find(x => x.name == option.ability.name);
                    Hermes.SpawnAbilities[Hermes.SpawnAbilities.IndexOf(ability)].SetInGame(true);

                    totalSlots[ability.loadout != -1 ? ability.loadout : 3].slots[ability.slot].GetComponent<DropSlot>().
                        OnDrop(loadoutOption.gameObject);
                    optionsObtained.Add(loadoutOption);
                    continue;
                }


                GO.SetActive(false);
                if (option.prerequisiteAbilities.Count > 0)
                    optionsLocked.Add(loadoutOption);
                else
                    optionsBank.Add(loadoutOption);
            }
        }
    }

    GameObject GenerateOptionInstance(LoadoutManager.Option option)
    {
        // Instantiate object
        GameObject newInstance = null;
        if (option.secondaryAbility.Length <= 0)
            newInstance = Instantiate(optionPrefab);
        else
        {
            newInstance = Instantiate(heavyOptionPrefab);
            newInstance.GetComponent<BigLoadoutOption>().SetSecondaryAbility(option.secondaryAbility);
        }

        // Set sibling order
        newInstance.transform.SetParent(transform);
        if (option.secondaryAbility.Length > 0)
            newInstance.transform.SetSiblingIndex(siblingBaseCount);

        // Setup loadout option
        LoadoutOption loadoutOption = newInstance.GetComponent<LoadoutOption>();
        loadoutOption.Ability = option.ability;
        loadoutOption.isPassive = option.isPassive;
        loadoutOption.PrerequisiteAbilities = new List<string>(option.prerequisiteAbilities);
        loadoutOption.OnInsert += ChooseOption;
        loadoutOption.GetComponent<DragDrop>().OnClick += () => StartCoroutine("InstructionsAnimation");

        return newInstance;
    }


    public void LevelUp()
    {
        NewAbilityText.SetActive(true);
        NewAbilitySquare.SetActive(true);
        SetLoweredMenu(false);

        for (int i = 0; i < 3; i++) {
            if (optionsBank.Count <= 0)
                return;

            int rnd = Random.Range(0, optionsBank.Count);

            optionsBank[rnd].gameObject.SetActive(true);
            optionsShown.Add(optionsBank[rnd]);
            optionsBank[rnd].GetComponent<RectTransform>().anchoredPosition = new Vector2(-40 + i*258, 166);
            if (optionsBank[rnd].GetComponent<BigLoadoutOption>())
                SetLoweredMenu(true);

            optionsBank.RemoveAt(rnd);
        }
    }

    void SetLoweredMenu(bool loweredMenu)
    {
        if (this.loweredMenu == loweredMenu)
            return;

        this.loweredMenu = loweredMenu;
        foreach (RectTransform rectTransform in GetComponentsInChildren<RectTransform>())
        {
            if (rectTransform.gameObject.name == "Background")
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
            string displayName = option.Ability.GetComponent<Ability>() ? option.Ability.GetComponent<Ability>().DisplayName : option.Ability.name;
            if (potentialToUnlock.PrerequisiteAbilities.Contains(displayName))
            {
                potentialToUnlock.PrerequisiteAbilities.Remove(displayName);
                if (potentialToUnlock.PrerequisiteAbilities.Count <= 0)
                {
                    optionsToDelete.Add(potentialToUnlock);
                    optionsBank.Add(potentialToUnlock);
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
            optionsBank.Add(shownOption);
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
}
