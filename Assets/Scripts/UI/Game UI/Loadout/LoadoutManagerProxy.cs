using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadoutManagerProxy : ILoadoutManager
{
    LoadoutManager trueLoadoutManager;

    // Start is called before the first frame update
    void Start()
    {
        trueLoadoutManager = ActorsManager.AM.GetPlayer().GetComponent<LoadoutManager>();
    }

    public override Device GetCurrentDevice()
    {
        return trueLoadoutManager.GetCurrentDevice();
    }

    public override Ability[] GetCurrentLoadout()
    {
        return trueLoadoutManager.GetCurrentLoadout();
    }

    public override void SetAbility(Ability ability, int loadout, int abilityNumber)
    {
        trueLoadoutManager.SetAbility(ability, loadout, abilityNumber);
    }

    public override void SetPassive(GameObject passive, bool isActivated)
    {
        trueLoadoutManager.SetPassive(passive, isActivated);
    }

    public override void SwitchDevice(int DeviceIndex)
    {
        trueLoadoutManager.SwitchDevice(DeviceIndex);
    }

    public override void SwitchDevice(Device Device, bool forceSwitch)
    {
        trueLoadoutManager.SwitchDevice(Device, forceSwitch);
    }

    public override List<LoadoutManager.Option> GetInitialOptions()
    {
        return trueLoadoutManager.GetInitialOptions();
    }

    public override List<LoadoutManager.Option> GetOptions()
    {
        return trueLoadoutManager.GetOptions();
    }

    public override void SubscribeToLoadoutSwitch(UnityAction subscriber)
    {
        trueLoadoutManager.SubscribeToLoadoutSwitch(subscriber);
    }

    public override void SubscribeToDeviceSwitch(UnityAction<Device> subscriber)
    {
        trueLoadoutManager.SubscribeToDeviceSwitch(subscriber);
    }

    public override int GetCurrentLoadoutIndex()
    {
        return trueLoadoutManager.GetCurrentLoadoutIndex();
    }
}
