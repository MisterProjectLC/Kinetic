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

    [Header("General")]
    [Tooltip("List of currently equipped loadouts")]
    public Loadout[] Loadouts;

    public bool AbilitiesEnabled = true;

    [Header("Cooldown timers")]
    [Tooltip("Time to put the weapon down")]
    public float DownCooldown = 0.1f;

    [Tooltip("Time to put the weapon up")]
    public float UpCooldown = 0.1f;


    [Header("Positions")]
    [Tooltip("Down position")]
    public Transform DownTransform;

    [Tooltip("Up position")]
    public Transform UpTransform;

    private int currentLoadout = 0;
    private int lastLoadout = 0;
    private Weapon currentWeapon;
    private Weapon newWeapon;

    private float animProgression = 0f;

    public UnityAction<Weapon> OnWeaponSwitch;
    public UnityAction OnLoadoutSwitch;

    [HideInInspector]
    public AnimationStage AnimStage { get; private set; } = AnimationStage.WeaponReady;
    public enum AnimationStage {
        WeaponDown,
        WeaponUp,
        WeaponReady
    }

    // References
    PlayerInputHandler m_InputHandler;


    void Awake()
    {
        m_InputHandler = GetComponent<PlayerInputHandler>();
        foreach (Ability ab in GetCurrentLoadout())
            if (ab is WeaponAbility)
            {
                currentWeapon = ((WeaponAbility)ab).WeaponRef;
                break;
            }
    }


    void Update()
    {
        ActivateAbilities();
        ManageLoadouts();
        ManageWeapons();
    }


    public void ActivateAbilities()
    {
        if (!AbilitiesEnabled)
            return;

        for (int i = 0; i < GetCurrentLoadout().Length; i++)
            if (m_InputHandler.GetAbilityDown(i + 1) || (m_InputHandler.GetAbility(i + 1) && GetCurrentLoadout()[i].HoldAbility))
            {
                if (AnimStage != AnimationStage.WeaponReady)
                    return;

                // Activate
                GetCurrentLoadout()[i].Activate();

                // Switch weapons
                if (GetCurrentLoadout()[i] is WeaponAbility)
                    SwitchWeapon(i);
            }
    }


    public void SwitchWeapon(int weaponIndex)
    {
        SwitchWeapon(((WeaponAbility)GetCurrentLoadout()[weaponIndex]).WeaponRef, false);
    }

    public void SwitchWeapon(Weapon weapon, bool forceSwitch)
    {
        newWeapon = weapon;
        if (newWeapon != currentWeapon || forceSwitch)
        {
            AnimStage = AnimationStage.WeaponDown;
            animProgression = DownCooldown;
        }

        OnWeaponSwitch.Invoke(weapon);
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
            Weapon weapon = null;
            foreach (Ability ab in GetCurrentLoadout())
                if (ab is WeaponAbility)
                {
                    weapon = ((WeaponAbility)ab).WeaponRef;
                    break;
                }

            if (weapon == null)
                Debug.Log("Loadout lacks a weapon");
            OnLoadoutSwitch.Invoke();
            SwitchWeapon(weapon, true);
        }
    }


    public void ManageWeapons()
    {
        // Animation End
        if (animProgression <= 0f)
        {
            switch (AnimStage)
            {
                case AnimationStage.WeaponDown:
                    AnimStage = AnimationStage.WeaponUp;
                    animProgression = UpCooldown;
                    if (currentWeapon != null)
                        currentWeapon.gameObject.SetActive(false);
                    currentWeapon = newWeapon;
                    if (currentWeapon != null)
                        currentWeapon.gameObject.SetActive(true);
                    break;

                case AnimationStage.WeaponUp:
                    AnimStage = AnimationStage.WeaponReady;
                    break;
            }
        }

        // Animation Progress
        else
        {
            if (currentWeapon != null)
                switch (AnimStage)
                {
                    case AnimationStage.WeaponDown:
                        currentWeapon.gameObject.transform.position = Vector3.Lerp(UpTransform.position, DownTransform.position,
                            (DownCooldown - animProgression) / DownCooldown);
                        currentWeapon.gameObject.transform.rotation = Quaternion.Lerp(UpTransform.rotation, DownTransform.rotation,
                            (DownCooldown - animProgression) / DownCooldown);
                        break;

                    case AnimationStage.WeaponUp:
                        currentWeapon.gameObject.transform.position = Vector3.Lerp(DownTransform.position, UpTransform.position,
                            (UpCooldown - animProgression) / UpCooldown);
                        currentWeapon.gameObject.transform.rotation = Quaternion.Lerp(DownTransform.rotation, UpTransform.rotation,
                            (UpCooldown - animProgression) / UpCooldown);
                        break;
                }

            animProgression -= Time.deltaTime;
        }
    }


    public Ability[] GetCurrentLoadout()
    {
        return Loadouts[currentLoadout].abilities;
    }


    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
