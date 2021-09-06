using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    [System.Serializable]
    public struct Option
    {
        public GameObject ability;
        public bool isPassive;
        public bool isHeavy;
    }

    [Header("Prefab References")]
    public GameObject optionPrefab;
    public GameObject heavyOptionPrefab;

    [SerializeField]
    List<Option> options;
    List<LoadoutOption> optionsBank;
    List<LoadoutOption> optionsShown;

    bool loweredMenu = false;
    int siblingBaseCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        optionsBank = new List<LoadoutOption>();
        optionsShown = new List<LoadoutOption>();
        siblingBaseCount = transform.GetChildCount();

        foreach (Option option in options)
        {
            GameObject newInstance = null;
            if (!option.isHeavy)
                newInstance = Instantiate(optionPrefab);
            else
                newInstance = Instantiate(heavyOptionPrefab);
            

            LoadoutOption loadoutOption = newInstance.GetComponent<LoadoutOption>();
            newInstance.transform.SetParent(transform);
            if (option.isHeavy)
                newInstance.transform.SetSiblingIndex(siblingBaseCount);

            loadoutOption.Ability = option.ability;
            loadoutOption.isPassive = option.isPassive;
            loadoutOption.OnInsert += ChooseOption;
            optionsBank.Add(loadoutOption);

            newInstance.SetActive(false);
        }
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
            if (optionsBank[rnd].isHeavy)
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