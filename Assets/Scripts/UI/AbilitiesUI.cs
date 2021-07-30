using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesUI : MonoBehaviour
{
    LoadoutManager loadoutManager;
    public Image[] SkillImages;

    Vector2 AbilitySize;

    // Start is called before the first frame update
    void Start()
    {
        AbilitySize = SkillImages[0].GetComponentInChildren<Image>().GetComponent<RectTransform>().sizeDelta;

        loadoutManager = ActorsManager.Player.GetComponent<LoadoutManager>();
        loadoutManager.OnWeaponSwitch += WeaponUpdate;
        loadoutManager.OnLoadoutSwitch += LoadoutUpdate;
        LoadoutUpdate();
    }


    private void Update()
    {
        for (int i = 0; i < loadoutManager.GetCurrentLoadout().Length; i++)
        {
            Ability currentAbility = loadoutManager.GetCurrentLoadout()[i];
            if (!(currentAbility is WeaponAbility))
                SkillImages[i].GetComponentInChildren<Image>().GetComponent<RectTransform>().sizeDelta =
                    AbilitySize * new Vector2(1f - (currentAbility.Timer/currentAbility.Cooldown), 1f);
        }
    }


    void WeaponUpdate(Weapon weapon)
    {
        foreach (Image image in SkillImages)
        {
            Color color = image.color;
            if (image.GetComponentInChildren<Text>().text == weapon.gameObject.name)
                image.color = new Color(color.r, color.g, color.b, 1);
            else
                image.color = new Color(color.r, color.g, color.b, 0.4f);

        }   
    }


    void LoadoutUpdate()
    {
        foreach (Image image in SkillImages)
            image.gameObject.SetActive(false);

        Ability[] abilities = loadoutManager.GetCurrentLoadout();
        int skillCount = 0;
        foreach (Ability ability in abilities)
        {
            SkillImages[skillCount].gameObject.SetActive(true);
            SkillImages[skillCount].GetComponentInChildren<Text>().text = ability.DisplayName;
            if (!(ability is WeaponAbility))
                SkillImages[skillCount].GetComponentInChildren<Image>().GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            else
                SkillImages[skillCount].GetComponentInChildren<Image>().GetComponent<RectTransform>().sizeDelta = AbilitySize;
            skillCount++;
        }
        WeaponUpdate(loadoutManager.GetCurrentWeapon());
    }

    
}
