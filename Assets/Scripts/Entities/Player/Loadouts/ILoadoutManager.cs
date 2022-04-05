using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ILoadoutManager : MonoBehaviour
{
    public abstract void SubscribeToLoadoutSwitch(UnityAction subscriber);

    public abstract void SubscribeToDeviceSwitch(UnityAction<Device> subscriber);

    public abstract void SwitchDevice(int DeviceIndex);

    public abstract void SwitchDevice(Device Device, bool forceSwitch);

    public abstract void SetAbility(Ability ability, int loadout, int abilityNumber);

    public abstract void SetPassive(GameObject passive, bool isActivated);

    public abstract Ability[] GetCurrentLoadout();

    public abstract int GetCurrentLoadoutIndex();

    public abstract Device GetCurrentDevice();

    public abstract List<LoadoutManager.Option> GetInitialOptions();

    public abstract List<LoadoutManager.Option> GetOptions();
}
