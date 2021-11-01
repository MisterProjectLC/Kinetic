using System.Collections;
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
    Loadout[] Loadouts = new Loadout[3];
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
    [SerializeField]
    Transform DownTransform;

    [Tooltip("Up position")]
    [SerializeField]
    Transform UpTransform;

    [SerializeField]
    Transform DeviceHolder;


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


    private void Awake()
    {
        for (int i = 0; i < Loadouts.Length; i++)
            Loadouts[i].abilities = new Ability[4];
    }


    void Start()
    {
        m_InputHandler = GetComponent<PlayerInputHandler>();
        StartCoroutine("StartDelayed");
    }


    IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        foreach (Ability ab in GetCurrentLoadout())
            if (ab && ab.GetComponent<Device>())
            {
                currentDevice = ab.GetComponent<Device>();
                break;
            }

        AnimStage = AnimationStage.DeviceUp;
        animProgression = UpCooldown;
        AttachCurrentToDeviceHolder();
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

        if (Device)
            Device.gameObject.SetActive(true);

        if (OnDeviceSwitch != null)
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
                        DeviceHolder.position = DownTransform.position;
                        DeviceHolder.rotation = DownTransform.rotation;
                        if (currentDevice.GetComponent<WeaponAbility>())
                            currentDevice.GetComponent<WeaponAbility>().ResetCooldown();
                        currentDevice.gameObject.SetActive(false);

                        currentDevice.transform.SetParent(DeviceHolder.parent);
                    }
                    currentDevice = newDevice;
                    AttachCurrentToDeviceHolder();
                    break;

                case AnimationStage.DeviceUp:
                    AnimStage = AnimationStage.DeviceReady;
                    DeviceHolder.position = UpTransform.position;
                    DeviceHolder.rotation = UpTransform.rotation;
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
                        DeviceHolder.position = Vector3.Lerp(UpTransform.position, DownTransform.position,
                            (DownCooldown - animProgression) / DownCooldown);
                        DeviceHolder.rotation = Quaternion.Lerp(UpTransform.rotation, DownTransform.rotation,
                            (DownCooldown - animProgression) / DownCooldown);
                        break;

                    case AnimationStage.DeviceUp:
                        DeviceHolder.position = Vector3.Lerp(DownTransform.position, UpTransform.position,
                            (UpCooldown - animProgression) / UpCooldown);
                        DeviceHolder.rotation = Quaternion.Lerp(DownTransform.rotation, UpTransform.rotation,
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

        //Debug.Log(loadout + ", " + abilityNumber + ": " + ability.name);
        //Debug.Log(Loadouts[loadout].abilities[abilityNumber].gameObject.name);

        if (OnLoadoutSwitch != null)
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


    void AttachCurrentToDeviceHolder()
    {
        if (currentDevice)
        {
            currentDevice.transform.SetParent(DeviceHolder);
            currentDevice.transform.position = DeviceHolder.position;
            currentDevice.transform.rotation = DeviceHolder.rotation;
        }
    }
}
