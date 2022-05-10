using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadoutManagerProxy : ILoadoutManager
{
    LoadoutManager trueLoadoutManager;
    bool init = false;

    // Start is called before the first frame update
    void Start()
    {
        if (init)
            return;
        init = true;

        trueLoadoutManager = ActorsManager.AM.GetPlayer().GetComponent<LoadoutManager>();
    }

    public override Device GetCurrentDevice()
    {
        Start();
        return trueLoadoutManager.GetCurrentDevice();
    }

    public override Ability[] GetCurrentLoadout()
    {
        Start();
        return trueLoadoutManager.GetCurrentLoadout();
    }

    public override void SetAbility(Ability ability, int loadout, int abilityNumber)
    {
        Start();
        trueLoadoutManager.SetAbility(ability, loadout, abilityNumber);
    }

    public override void SetPassive(Upgrade passive, bool isActivated)
    {
        Start();
        trueLoadoutManager.SetPassive(passive, isActivated);
    }

    public override void SwitchDevice(int DeviceIndex)
    {
        Start();
        trueLoadoutManager.SwitchDevice(DeviceIndex);
    }

    public override void SwitchDevice(Device Device, bool forceSwitch)
    {
        Start();
        trueLoadoutManager.SwitchDevice(Device, forceSwitch);
    }

    public override List<Option> GetInitialOptions()
    {
        Start();
        return trueLoadoutManager.GetInitialOptions();
    }

    public override List<Option> GetInitialPassives()
    {
        Start();
        return trueLoadoutManager.GetInitialPassives();
    }

    public override List<Option> GetOptions()
    {
        Start();
        return trueLoadoutManager.GetOptions();
    }

    public override void SubscribeToLoadoutSwitch(UnityAction subscriber)
    {
        Start();
        trueLoadoutManager.SubscribeToLoadoutSwitch(subscriber);
    }

    public override void SubscribeToDeviceSwitch(UnityAction<Device> subscriber)
    {
        Start();
        trueLoadoutManager.SubscribeToDeviceSwitch(subscriber);
    }

    public override void SubscribeToNewUpgrade(UnityAction<int, int, Upgrade> subscriber)
    {
        Start();
        trueLoadoutManager.SubscribeToNewUpgrade(subscriber);
    }

    public override int GetCurrentLoadoutIndex()
    {
        Start();
        return trueLoadoutManager.GetCurrentLoadoutIndex();
    }

    public override Ability[] GetLoadout(int loadout)
    {
        Start();
        return trueLoadoutManager.GetLoadout(loadout);
    }

    public override void SetCurrentLoadout(int loadout)
    {
        Start();
        SetCurrentLoadout(loadout);
    }
}
