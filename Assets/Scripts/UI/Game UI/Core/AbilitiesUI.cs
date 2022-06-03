using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUI : MonoBehaviour
{
    [SerializeField]
    ILoadoutManager loadoutManager;

    [SerializeField]
    AbilityView[] AbilityViews;
    Ability[] abilities = new Ability[4];

    [SerializeField]
    AmmoIndicator ammoIndicator;

    [SerializeField]
    RectTransform centerTransform;

    Dictionary<Ability, GameObject> additionalAbilityViews = new Dictionary<Ability, GameObject>(4);
    List<GameObject> additionalAbilityViewsList = new List<GameObject>(4);

    [SerializeField]
    GameObjectReference PlayerReference;

    // Start is called before the first frame update
    void Start()
    {
        foreach (AbilityView view in AbilityViews)
            view.CenterTransform = centerTransform;

        ammoIndicator.Setup(PlayerReference.Reference);

        loadoutManager.SubscribeToDeviceSwitch(DeviceUpdate);
        loadoutManager.SubscribeToLoadoutSwitch(LoadoutUpdate);
        LoadoutUpdate();
    }


    private void Update()
    {
        for (int i = 0; i < Mathf.Min(abilities.Length, AbilityViews.Length); i++)
        {
            if (!abilities[i])
                continue;
            AbilityViews[i].AbilityUpdate(abilities[i]);

            if (abilities[i] is WeaponAbility)
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
