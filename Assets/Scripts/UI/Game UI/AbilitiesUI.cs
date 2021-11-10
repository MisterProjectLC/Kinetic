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
        AbilitySize = SkillImages[0].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;

        loadoutManager = ActorsManager.Player.GetComponent<LoadoutManager>();
        loadoutManager.OnDeviceSwitch += DeviceUpdate;
        loadoutManager.OnLoadoutSwitch += LoadoutUpdate;
        LoadoutUpdate();
    }


    private void Update()
    {
        for (int i = 0; i < loadoutManager.GetCurrentLoadout().Length; i++)
        {
            Ability currentAbility = loadoutManager.GetCurrentLoadout()[i];
            if (currentAbility)
                SkillImages[i].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta =
                    AbilitySize * new Vector2(1f - (currentAbility.Timer/currentAbility.Cooldown), 1f);
        }
    }


    void DeviceUpdate(Device device)
    {
        if (!device)
            return;

        foreach (Image image in SkillImages)
        {
            Color color = image.color;
            if (image.GetComponentInChildren<Text>().text == device.gameObject.name)
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
        for (int i = 0; i < SkillImages.Length; i++)
        {
            if (abilities[i] == null)
            {
                SkillImages[i].gameObject.SetActive(false);
                continue;
            }

            SkillImages[i].gameObject.SetActive(true);
            SkillImages[i].GetComponentInChildren<Text>().text = abilities[i].DisplayName;
            SkillImages[i].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }

        Device currentDevice = loadoutManager.GetCurrentDevice();
        if (currentDevice)
            DeviceUpdate(currentDevice);
    }

    
}
