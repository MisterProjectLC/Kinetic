using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadoutManager : MonoBehaviour
{
    // Loadouts
    [System.Serializable]
    public struct Loadout
    {
        public Ability[] abilities;
    }

    [System.Serializable]
    public struct Option
    {
        public GameObject ability;
        public bool isPassive;
        [Tooltip("Leave empty if none")]
        public string secondaryAbility;
    }

    [Header("General")]
    [Tooltip("List of currently equipped loadouts")]
    [HideInInspector]
    public Loadout[] Loadouts;
    public List<Option> InitialOptions;
    public List<Option> Options;

    [HideInInspector]
    public bool AbilitiesEnabled = true;

    [Header("Cooldown timers")]
    [Tooltip("Time to put the Device down")]
    public float DownCooldown = 0.1f;

    [Tooltip("Time to put the Device up")]
    public float UpCooldown = 0.1f;


    [Header("Positions")]
    [Tooltip("Down position")]
    public Transform DownTransform;

    [Tooltip("Up position")]
    public Transform UpTransform;

    private int currentLoadout = 0;
    private int lastLoadout = 0;
    private Device currentDevice;
    private Device newDevice;

    private float animProgression = 0f;

    public UnityAction<Device> OnDeviceSwitch;
    public UnityAction OnLoadoutSwitch;

    [HideInInspector]
    public AnimationStage AnimStage { get; private set; } = AnimationStage.DeviceReady;
    public enum AnimationStage {
        DeviceDown,
        DeviceUp,
        DeviceReady
    }

    // References
    PlayerInputHandler m_InputHandler;


    void Start()
    {
        m_InputHandler = GetComponent<PlayerInputHandler>();
        foreach (Ability ab in GetCurrentLoadout())
            if (ab && ab.GetComponent<Device>())
            {
                currentDevice = ab.GetComponent<Device>();
                break;
            }
    }


    void Update()
    {
        ActivateAbilities();
        ManageLoadouts();
        ManageDevices();
    }


    public void ActivateAbilities()
    {
        if (!AbilitiesEnabled)
            return;

        for (int i = 0; i < GetCurrentLoadout().Length; i++)
        {
            if (!GetCurrentLoadout()[i])
                continue;

            bool down = m_InputHandler.GetAbilityDown(i + 1);
            bool hold = (m_InputHandler.GetAbility(i + 1) && GetCurrentLoadout()[i].HoldAbility);
            bool up = (m_InputHandler.GetAbilityUp(i + 1) && GetCurrentLoadout()[i].ReleaseAbility);

            if (down || hold || up)
            {
                if (AnimStage != AnimationStage.DeviceReady)
                    return;

                // Switch Devices
                if (GetCurrentLoadout()[i].GetComponent<Device>())
                    SwitchDevice(i);

                // Activate
                GetCurrentLoadout()[i].Activate(down || hold ? Ability.Input.ButtonDown : Ability.Input.ButtonUp);
            }
        }
    }


    public void SwitchDevice(int DeviceIndex)
    {
        SwitchDevice(GetCurrentLoadout()[DeviceIndex].GetComponent<Device>(), false);
    }

    public void SwitchDevice(Device Device, bool forceSwitch)
    {
        newDevice = Device;
        if (newDevice != currentDevice || forceSwitch)
        {
            AnimStage = AnimationStage.DeviceDown;
            animProgression = DownCooldown;
        }
        Device.gameObject.SetActive(true);

        OnDeviceSwitch.Invoke(Device);
    }


    public void ManageLoadouts()
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
            OnLoadoutSwitch.Invoke();
            SwitchDevice(Device, true);
        }
    }


    public void ManageDevices()
    {
        // Animation End
        if (animProgression <= 0f)
        {
            switch (AnimStage)
            {
                case AnimationStage.DeviceDown:
                    AnimStage = AnimationStage.DeviceUp;
                    animProgression = UpCooldown;
                    if (currentDevice != null)
                    {
                        if (currentDevice.GetComponent<WeaponAbility>())
                            currentDevice.GetComponent<WeaponAbility>().ResetCooldown();
                        currentDevice.gameObject.SetActive(false);
                    }
                    currentDevice = newDevice;
                    break;

                case AnimationStage.DeviceUp:
                    AnimStage = AnimationStage.DeviceReady;
                    break;
            }
        }

        // Animation Progress
        else
        {
            if (currentDevice != null)
                switch (AnimStage)
                {
                    case AnimationStage.DeviceDown:
                        currentDevice.gameObject.transform.position = Vector3.Lerp(UpTransform.position, DownTransform.position,
                            (DownCooldown - animProgression) / DownCooldown);
                        currentDevice.gameObject.transform.rotation = Quaternion.Lerp(UpTransform.rotation, DownTransform.rotation,
                            (DownCooldown - animProgression) / DownCooldown);
                        break;

                    case AnimationStage.DeviceUp:
                        currentDevice.gameObject.transform.position = Vector3.Lerp(DownTransform.position, UpTransform.position,
                            (UpCooldown - animProgression) / UpCooldown);
                        currentDevice.gameObject.transform.rotation = Quaternion.Lerp(DownTransform.rotation, UpTransform.rotation,
                            (UpCooldown - animProgression) / UpCooldown);
                        break;
                }

            animProgression -= Time.deltaTime;
        }
    }


    public void SetAbility(GameObject ability, int loadout, int abilityNumber)
    {
        if (ability != null)
            Loadouts[loadout].abilities[abilityNumber] = ability.GetComponent<Ability>();
        else
            Loadouts[loadout].abilities[abilityNumber] = null;

        OnLoadoutSwitch.Invoke();
    }

    public void SetPassive(GameObject passive, bool isActivated)
    {
        passive.SetActive(isActivated);
    }


    public Ability[] GetCurrentLoadout()
    {
        return Loadouts[currentLoadout].abilities;
    }


    public Device GetCurrentDevice()
    {
        return currentDevice;
    }
}
