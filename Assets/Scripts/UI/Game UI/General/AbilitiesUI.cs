using UnityEngine;
using UnityEngine.UI;

public class AbilitiesUI : MonoBehaviour
{
    LoadoutManager loadoutManager;
    [SerializeField]
    AbilityView[] AbilityViews;
    Ability[] abilities;

    [SerializeField]
    AmmoIndicator ammoIndicator;

    // Start is called before the first frame update
    void Start()
    {
        ammoIndicator.Setup(ActorsManager.Player.gameObject);

        loadoutManager = ActorsManager.Player.GetComponent<LoadoutManager>();
        loadoutManager.OnDeviceSwitch += DeviceUpdate;
        loadoutManager.OnLoadoutSwitch += LoadoutUpdate;
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
