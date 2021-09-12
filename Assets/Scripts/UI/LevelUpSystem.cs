using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    [System.Serializable]
    public struct Option
    {
        public GameObject ability;
        public bool isPassive;
        [Tooltip("Leave empty if none")]
        public string secondaryAbility;
    }

    [SerializeField]
    List<RectTransform> initialSlots;

    [Header("Prefab References")]
    public GameObject optionPrefab;
    public GameObject heavyOptionPrefab;

    List<LoadoutOption> optionsBank;
    List<LoadoutOption> optionsShown;

    bool loweredMenu = false;
    int siblingBaseCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        LoadoutManager loadout = ActorsManager.Player.GetComponent<LoadoutManager>();
        optionsBank = new List<LoadoutOption>();
        optionsShown = new List<LoadoutOption>();
        siblingBaseCount = transform.GetChildCount();

        int i = 0;
        foreach (Option option in loadout.InitialOptions)
        {
            LoadoutOption loadoutOption = GenerateOptionInstance(option).GetComponent<LoadoutOption>();
            DropSlot dropSlot = initialSlots[i].GetComponent<DropSlot>();
            dropSlot.OnDrop(loadoutOption.gameObject);
            i++;
            if (option.secondaryAbility.Length > 0)
                i++;
        }

        foreach (Option option in loadout.Options)
        {
            GameObject GO = GenerateOptionInstance(option);
            GO.SetActive(false);
            LoadoutOption loadoutOption = GO.GetComponent<LoadoutOption>();
            optionsBank.Add(loadoutOption);
        }
    }

    GameObject GenerateOptionInstance(Option option)
    {
        GameObject newInstance = null;
        if (option.secondaryAbility.Length <= 0)
            newInstance = Instantiate(optionPrefab);
        else
        {
            newInstance = Instantiate(heavyOptionPrefab);
            newInstance.GetComponent<BigLoadoutOption>().SetSecondaryAbility(option.secondaryAbility);
        }

        LoadoutOption loadoutOption = newInstance.GetComponent<LoadoutOption>();
        newInstance.transform.SetParent(transform);
        if (option.secondaryAbility.Length > 0)
            newInstance.transform.SetSiblingIndex(siblingBaseCount);

        loadoutOption.Ability = option.ability;
        loadoutOption.isPassive = option.isPassive;
        loadoutOption.OnInsert += ChooseOption;

        return newInstance;
    }


    public void LevelUp()
    {
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

        optionsShown.Remove(option);
        foreach (LoadoutOption shownOption in optionsShown)
        {
            shownOption.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, 1000);
            shownOption.gameObject.SetActive(false);
            optionsBank.Add(shownOption);
        }
        optionsShown.Clear();
    }
}