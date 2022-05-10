using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ILoadoutManager : MonoBehaviour
{
    [System.Serializable]
    public struct Option
    {
        public Upgrade option;
        public bool isPassive
        {
            get
            {
                return option is Passive;
            }
        }

        [Tooltip("Leave empty if none")]
        public LocalizedString secondaryAbility;
        public List<LocalizedString> prerequisiteAbilities;
    }

    public abstract void SubscribeToLoadoutSwitch(UnityAction subscriber);

    public abstract void SubscribeToDeviceSwitch(UnityAction<Device> subscriber);

    public abstract void SubscribeToNewUpgrade(UnityAction<int, int, Upgrade> subscriber);

    public abstract void SwitchDevice(int DeviceIndex);

    public abstract void SwitchDevice(Device Device, bool forceSwitch);

    public abstract void SetAbility(Ability ability, int loadout, int abilityNumber);

    public abstract void SetPassive(Upgrade passive, bool isActivated);

    public abstract void SetCurrentLoadout(int loadout);

    public abstract Ability[] GetCurrentLoadout();

    public abstract Ability[] GetLoadout(int loadout);

    public abstract int GetCurrentLoadoutIndex();

    public abstract Device GetCurrentDevice();

    public abstract List<Option> GetInitialOptions();

    public abstract List<Option> GetInitialPassives();

    public abstract List<Option> GetOptions();
}
