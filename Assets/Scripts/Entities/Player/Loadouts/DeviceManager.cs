using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour
{
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
    public Device CurrentDevice { get; set; }
    private Device newDevice;


    public AnimationStage AnimStage { get; private set; } = AnimationStage.DeviceReady;
    public enum AnimationStage
    {
        DeviceDown,
        DeviceUp,
        DeviceReady
    }

    float animProgression = 0f;

    void Start()
    {
        StartCoroutine(StartDelayed());
    }


    IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        AnimStage = AnimationStage.DeviceUp;
        animProgression = UpCooldown;
        AttachCurrentToDeviceHolder();
    }

    public void SwitchDevice(Device Device, bool forceSwitch)
    {
        newDevice = Device;
        if (newDevice != CurrentDevice || forceSwitch)
        {
            AnimStage = AnimationStage.DeviceDown;
            animProgression = DownCooldown;
        }

        if (Device)
            Device.gameObject.SetActive(true);
    }


    public void UpdateDevices()
    {
        // Animation End
        if (animProgression <= 0f)
        {
            switch (AnimStage)
            {
                case AnimationStage.DeviceDown:
                    AnimStage = AnimationStage.DeviceUp;
                    animProgression = UpCooldown;
                    if (CurrentDevice != null)
                    {
                        DeviceHolder.position = DownTransform.position;
                        DeviceHolder.rotation = DownTransform.rotation;
                        if (CurrentDevice.GetComponent<WeaponAbility>())
                            CurrentDevice.GetComponent<WeaponAbility>().ResetCooldown();
                        CurrentDevice.gameObject.SetActive(false);

                        CurrentDevice.transform.SetParent(DeviceHolder.parent);
                    }
                    CurrentDevice = newDevice;
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
            if (CurrentDevice != null)
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


    void AttachCurrentToDeviceHolder()
    {
        if (CurrentDevice)
        {
            CurrentDevice.transform.SetParent(DeviceHolder);
            CurrentDevice.transform.position = DeviceHolder.position;
            CurrentDevice.transform.rotation = DeviceHolder.rotation;
        }
    }

    public bool IsCurrentDeviceReady()
    {
        return AnimStage != AnimationStage.DeviceReady;
    }
}
