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
    }

    public GameObject optionPrefab;

    [SerializeField]
    List<Option> options;
    List<LoadoutOption> optionsBank;
    List<LoadoutOption> optionsShown;

    // Start is called before the first frame update
    void Awake()
    {
        optionsBank = new List<LoadoutOption>();
        optionsShown = new List<LoadoutOption>();

        foreach (Option option in options)
        {
            GameObject newInstance = Instantiate(optionPrefab);
            newInstance.transform.SetParent(transform);

            LoadoutOption loadoutOption = newInstance.GetComponent<LoadoutOption>();
            loadoutOption.Ability = option.ability;
            loadoutOption.isPassive = option.isPassive;
            loadoutOption.OnInsert += ChooseOption;
            optionsBank.Add(loadoutOption);

            newInstance.SetActive(false);
        }
    }

    public void LevelUp()
    {
        for (int i = 0; i < 3; i++) {
            if (optionsBank.Count <= 0)
                return;

            int rnd = Random.Range(0, optionsBank.Count);
            optionsBank[rnd].gameObject.SetActive(true);
            optionsShown.Add(optionsBank[rnd]);
            optionsBank[rnd].GetComponent<RectTransform>().anchoredPosition = new Vector2(-132 + i*258, 226);
            optionsBank.RemoveAt(rnd);
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