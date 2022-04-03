using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SmartData.SmartFloat;

public class StyleMeter : MonoBehaviour
{
    public enum Categories
    {
        Kill,
        Multikill,
        ChainKill,
        Hurt,
        Movement,
        Variety,
        Airborne,
        HazardKill,
        Damage,
        Count
    }

    const int ABILITY_MEMORY = 5;
    List<Ability> lastAbilities = new List<Ability>(ABILITY_MEMORY);

    Health health;
    PlayerCharacterController player;
    Dictionary<Categories, LocalizedString> localizers = new Dictionary<Categories, LocalizedString>();

    public UnityAction OnUpdate;
    public UnityAction OnDeplete;
    UnityAction<bool> OnCritical;
    public void SubscribeToCritical(UnityAction<bool> subscribee) { OnCritical += subscribee; }

    public UnityAction<float, int, string> OnEvent;
    public UnityAction<int, bool> OnBonus;

    [SerializeField]
    FloatWriter currentStyle;
    public float CurrentStyle { get { return currentStyle; } }

    [SerializeField]
    FloatWriter maxStyle;
    public float MaxStyle { get { return maxStyle; } }


    [HideInInspector]
    public bool DrainActive = false;

    [SerializeField]
    SlowdownConfig config;

    float movementClock = 0f;
    float clock = 0f;

    bool airborne = false;
    public bool Critical { get; private set; }

    private void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        for (Categories i = Categories.Kill; i != Categories.Count; i++)
            if (i != Categories.Airborne)
                localizers.Add(i, new LocalizedString("style_" + i.ToString().ToLower()));

        clock = config.ComboMaxTime;
        SetJuiceLeft(MaxStyle);
        health = GetComponentInParent<Health>();
        health.OnDamageAttack += OnDamage;
        foreach (Attack attack in player.GetComponentsInChildren<Attack>())
        {
            attack.OnCritical += EnemyCritical;
            attack.OnKill += (Attack a, GameObject g, bool b) => StyleKill(g, b);
        }

        foreach (Ability ability in player.GetComponentsInChildren<Ability>())
            ability.OnExecuteAbility += AbilityUsage;
    }

    private void Update()
    {
        // Decay
        if (currentStyle > 0f && DrainActive)
            SpendJuice(Time.deltaTime * config.DepleteRate);

        // Combo time
        if (clock < config.CombatMaxTime)
            clock += Time.deltaTime;

        // Movement gain
        movementClock += Time.deltaTime;
        if (movementClock > 1f)
        {
            movementClock = 0f;
            if (player.GetMoveVelocity().magnitude > config.MovementSpeed)
                GainJuice(config.MovementGain, (int)Categories.Movement, localizers[Categories.Movement].value);
        }

        // Airborne bonus
        if (airborne == player.IsGrounded)
        {
            airborne = !player.IsGrounded;
            OnBonus?.Invoke((int)Categories.Airborne, airborne);
        }
    }


    void AbilityUsage(Ability ability)
    {
        int index = 0;
        for (int i = lastAbilities.Count-1; i >= 0; i--)
            if (lastAbilities[i] == ability)
            {
                index = i;
                lastAbilities.RemoveAt(i);
                break;
            }
        lastAbilities.Add(ability);

        if (clock < config.CombatMaxTime)
            GainJuice(config.VarietyGain * ((lastAbilities.Count - 1) - index), (int)Categories.Variety, localizers[Categories.Variety].value);
    }


    /*
    void EnemyHit(GameObject target, float multiplier, int damage)
    {
        StyleCrate styleCrate = target.GetComponent<Damageable>().GetHealth().GetComponent<StyleCrate>();

        if (styleCrate && styleCrate.StylePerDamage != 0f)
        {
            GainJuice(0.5f * Mathf.CeilToInt(styleCrate.StylePerDamage * damage),
                (int)Categories.Damage, "Damage");
        }
    }
    */

    void EnemyCritical(Health target)
    {
        StyleCrate styleCrate = target.GetComponent<StyleCrate>();

        if (styleCrate && styleCrate.StyleOnCritical != 0f)
            GainJuice(styleCrate.StyleOnCritical, (int)Categories.Damage, localizers[Categories.Damage].value);
    }

    void StyleKill(GameObject target, bool indirect)
    {
        DrainActive = true;

        if (indirect)
        {
            GainJuice(config.IndirectGain, (int)Categories.HazardKill, localizers[Categories.HazardKill].value);
            return;
        }

        if (!target.GetComponent<StyleCrate>())
            return;

        GainJuice(target.GetComponent<StyleCrate>().StyleOnKill, (int)Categories.Kill, localizers[Categories.Kill].value);
        if (clock < config.MultiMaxTime)
            GainJuice(config.MultiGain, (int)Categories.Multikill, localizers[Categories.Multikill].value);
        else if (clock < config.ComboMaxTime)
            GainJuice(config.ChainGain, (int)Categories.ChainKill, localizers[Categories.ChainKill].value);

        clock = 0f;
    }

    void OnDamage(int damage, Attack attack)
    {
        if (attack == null || attack.Agressor != GetComponent<Actor>())
            SpendJuice(damage * config.DamageLoss, (int)Categories.Hurt, localizers[Categories.Hurt].value);
    }


    public void GainJuice(float gain, int category = -1, string text = "")
    {
        if (gain == 0)
            return;

        if (text != "")
            OnEvent?.Invoke(gain, category, text);

        SetJuiceLeft(Mathf.Clamp(currentStyle + gain * (player.IsGrounded ? 1 : 1.5f), 0f, maxStyle));
    }


    public void SpendJuice(float cost, int category = -1, string text = "")
    {
        if (cost == 0)
            return;

        if (text != "")
            OnEvent?.Invoke(-cost, category, text);

        if (currentStyle - cost > 0f)
            SetJuiceLeft(currentStyle - cost);
        else
        {
            currentStyle.value = 0f;
            OnDeplete?.Invoke();
        }
    }


    void SetJuiceLeft(float juiceLeft)
    {
        currentStyle.value = juiceLeft;
        OnUpdate?.Invoke();

        bool lastCritical = Critical;
        Critical = juiceLeft > maxStyle * config.CriticalPercent;
        if (lastCritical != Critical)
            OnCritical?.Invoke(Critical);
    }


    public float GetCriticalLevel()
    {
        return config.CriticalPercent * maxStyle;
    }
}
