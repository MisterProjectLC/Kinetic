using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesUI : MonoBehaviour
{
    [SerializeField]
    ILoadoutManager loadoutManager;

    [SerializeField]
    AbilityView[] AbilityViews;
    Ability[] abilities;

    [SerializeField]
    AmmoIndicator ammoIndicator;

    [SerializeField]
    RectTransform centerTransform;

    Dictionary<Ability, GameObject> additionalAbilityViews = new Dictionary<Ability, GameObject>(4);
    List<GameObject> additionalAbilityViewsList = new List<GameObject>(4);

    // Start is called before the first frame update
    void Start()
    {
        foreach (AbilityView view in AbilityViews)
            view.CenterTransform = centerTransform;

        ammoIndicator.Setup(ActorsManager.Player.gameObject);

        loadoutManager.SubscribeToDeviceSwitch(DeviceUpdate);
        loadoutManager.SubscribeToLoadoutSwitch(LoadoutUpdate);
        LoadoutUpdate();
    }


    private void Update()
    {
        for (int i = 0; i < AbilityViews.Length; i++)
        {
            AbilityViews[i].AbilityUpdate(abilities[i]);

            if (abilities[i] && abilities[i] is WeaponAbility)
                ammoIndicator.SetCurrentWeapon(((WeaponAbility)abilities[i]).WeaponRef);
        }
    }

    void DeviceUpdate(Device device)
    {
        if (!device)
            return;

        foreach (AbilityView abilityView in AbilityViews)
            abilityView.DeviceUpdate(device);
    }


    void LoadoutUpdate()
    {
        abilities = loadoutManager.GetCurrentLoadout();

        for (int i = 0; i < AbilityViews.Length; i++)
            AbilityViews[i].LoadoutUpdate(abilities[i]);

        Device currentDevice = loadoutManager.GetCurrentDevice();
        if (currentDevice)
            DeviceUpdate(currentDevice);
    }

    
}
