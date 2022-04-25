using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadoutManager : ILoadoutManager
{
    // Loadouts
    [System.Serializable]
    public struct Loadout
    {
        public Ability[] abilities;
    }

    [Header("General")]
    [Tooltip("List of currently equipped loadouts")]
    [HideInInspector]
    Loadout[] Loadouts = new Loadout[3];
    [SerializeField]
    List<Option> InitialOptions;
    [SerializeField]
    List<Option> InitialPassives;
    [SerializeField]
    List<Option> Options;

    [HideInInspector]
    public bool AbilitiesEnabled = true;

    [HideInInspector]
    int currentLoadout = 0;
    int lastLoadout = 0;

    UnityAction<Device> OnDeviceSwitch;
    public override void SubscribeToDeviceSwitch(UnityAction<Device> subscriber) { OnDeviceSwitch += subscriber; }

    UnityAction OnLoadoutSwitch;
    public override void SubscribeToLoadoutSwitch(UnityAction subscriber) { OnLoadoutSwitch += subscriber; }

    #region References
    PlayerInputHandler m_InputHandler;
    DeviceManager m_DeviceManager;
    #endregion


    private void Awake()
    {
        m_DeviceManager = GetComponentInChildren<DeviceManager>();
        m_InputHandler = GetComponent<PlayerInputHandler>();
        for (int i = 0; i < Loadouts.Length; i++)
            Loadouts[i].abilities = new Ability[4];
    }


    void Start()
    {
        StartCoroutine(StartDelayed());
    }


    IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        foreach (Ability ab in GetCurrentLoadout())
            if (ab && ab.GetComponent<Device>())
            {
                m_DeviceManager.CurrentDevice = ab.GetComponent<Device>();
                break;
            }
    }
    


    void Update()
    {
        ActivateAbilities();
        ManageLoadouts();
        m_DeviceManager.UpdateDevices();
    }


    void ActivateAbilities()
    {
        if (!AbilitiesEnabled)
            return;

        for (int i = 0; i < GetCurrentLoadout().Length; i++)
        {
            Ability thisAbility = GetCurrentLoadout()[i];
            if (!thisAbility)
                continue;

            bool down = m_InputHandler.GetAbilityDown(i + 1);
            bool hold = (m_InputHandler.GetAbility(i + 1) && thisAbility.HoldAbility);
            bool up = (m_InputHandler.GetAbilityUp(i + 1) && thisAbility.ReleaseAbility);

            if (down || hold || up)
            {
                if (m_DeviceManager.IsCurrentDeviceReady())
                    return;

                // Switch Devices
                if (thisAbility.GetComponent<Device>())
                    SwitchDevice(i);

                // Activate
                thisAbility.Activate(down || hold ? Ability.Input.ButtonDown : Ability.Input.ButtonUp);
            }
        }
    }


    public override void SwitchDevice(int DeviceIndex)
    {
        SwitchDevice(GetCurrentLoadout()[DeviceIndex].GetComponent<Device>(), false);
    }

    public override void SwitchDevice(Device Device, bool forceSwitch)
    {
        m_DeviceManager.SwitchDevice(Device, forceSwitch);

        if (OnDeviceSwitch != null)
            OnDeviceSwitch.Invoke(Device);
    }


    void ManageLoadouts()
    {
        int loadoutButton = -1;
        if (m_InputHandler.GetSwitch())
            loadoutButton = lastLoadout;
        else
            loadoutButton = m_InputHandler.GetSelectLoadoutInput()-1;

        if (loadoutButton != -2 && loadoutButton != currentLoadout && loadoutButton < Loadouts.Length)
        {
            lastLoadout = currentLoadout;
            currentLoadout = loadoutButton;
            Device Device = null;
            foreach (Ability ab in GetCurrentLoadout())
                if (ab && ab.GetComponent<Device>())
                {
                    Device = ab.GetComponent<Device>();
                    break;
                }

            if (Device == null)
                Debug.Log("Loadout lacks a Device");
            OnLoadoutSwitch?.Invoke();
            SwitchDevice(Device, true);
        }
    }


    public override void SetAbility(Ability ability, int loadout, int abilityNumber)
    {
        // Unassign previous
        if (Loadouts[loadout].abilities[abilityNumber])
            Loadouts[loadout].abilities[abilityNumber].Assigned = false;

        // Assign new
        if (ability != null)
        {
            Loadouts[loadout].abilities[abilityNumber] = ability;
            ability.Assigned = true;
        }
        // Leave at null
        else
            Loadouts[loadout].abilities[abilityNumber] = null;

        //Debug.Log(loadout + ", " + abilityNumber + ": " + ability.name);
        //Debug.Log(Loadouts[loadout].abilities[abilityNumber].gameObject.name);

        OnLoadoutSwitch?.Invoke();
    }

    public override void SetPassive(Upgrade passive, bool isActivated)
    {
        passive.gameObject.SetActive(isActivated);
    }


    #region Setters
    public override Ability[] GetCurrentLoadout()
    {
        return Loadouts[currentLoadout].abilities;
    }

    public override int GetCurrentLoadoutIndex()
    {
        return currentLoadout;
    }

    public override Device GetCurrentDevice()
    {
        return m_DeviceManager.CurrentDevice;
    }

    public override List<Option> GetInitialOptions()
    {
        return InitialOptions;
    }

    public override List<Option> GetOptions()
    {
        return Options;
    }

    public override List<Option> GetInitialPassives()
    {
        return InitialPassives;
    }
    #endregion
}
