using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        IndirectKill,
        Damage
    }

    const int ABILITY_MEMORY = 5;
    List<Ability> lastAbilities = new List<Ability>(ABILITY_MEMORY);

    Health health;
    PlayerCharacterController player;

    public UnityAction OnUpdate;
    public UnityAction OnDeplete;
    public UnityAction<bool> OnCritical;
    public UnityAction<float, int, string> OnEvent;
    public UnityAction<int, bool> OnBonus;

    public float JuiceLeft { get; private set; }

    [SerializeField]
    public float JuiceMax { get; private set; } = 10f;

    [SerializeField]
    float CriticalPercent = 0.8f;
    [SerializeField]
    float DepleteRate = 0.25f;
    [HideInInspector]
    public bool DrainActive = false;

    [SerializeField]
    float MultiMaxTime = 0.25f;
    [SerializeField]
    float ComboMaxTime = 3f;
    [SerializeField]
    float MovementSpeed = 25f;

    [Tooltip("Gain amounts")]
    [SerializeField]
    float KillGain = 0.5f;
    [SerializeField]
    float ChainGain = 1.5f;
    [SerializeField]
    float MultiGain = 2f;
    [SerializeField]
    float IndirectGain = 5f;
    [SerializeField]
    float VarietyGain = 1f;
    [SerializeField]
    float MovementGain = 0.5f;

    float movementClock = 0f;
    float clock = 0f;

    bool airborne = false;
    public bool Critical { get; private set; }

    private void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();

        clock = ComboMaxTime;
        SetJuiceLeft(JuiceMax);
        health = GetComponentInParent<Health>();
        health.OnDamageAttack += OnDamage;
        foreach (Attack attack in player.GetComponentsInChildren<Attack>())
        {
            attack.OnAttack += EnemyHit;
            attack.OnKill += (Attack a, GameObject g, bool b) => StyleKill(g, b);
        }

        foreach (Ability ability in player.GetComponentsInChildren<Ability>())
            ability.OnExecuteAbility += AbilityUsage;
    }

    private void Update()
    {
        // Decay
        if (JuiceLeft > 0f && DrainActive)
            SpendJuice(Time.deltaTime * DepleteRate);

        // Combo time
        if (clock < ComboMaxTime)
            clock += Time.deltaTime;

        // Movement gain
        movementClock += Time.deltaTime;
        if (movementClock > 1f)
        {
            movementClock = 0f;
            if (player.MoveVelocity.magnitude > MovementSpeed)
                GainJuice(MovementGain, (int)Categories.Movement, "Movement");
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

        if (clock < ComboMaxTime)
            GainJuice(VarietyGain * ((lastAbilities.Count - 1) - index), (int)Categories.Variety, "Variety");
    }


    void EnemyHit(GameObject target, float multiplier, int damage)
    {
        StyleCrate styleCrate = target.GetComponent<Damageable>().GetHealth().GetComponent<StyleCrate>();

        if (styleCrate && styleCrate.StylePerDamage != 0f)
        {
            GainJuice(0.5f * Mathf.CeilToInt(styleCrate.StylePerDamage * damage),
                (int)Categories.Damage, "Damage");
        }
    }

    void StyleKill(GameObject target, bool indirect)
    {
        DrainActive = true;

        if (indirect)
        {
            GainJuice(IndirectGain, (int)Categories.IndirectKill, "Trap Kill");
            return;
        }

        if (!target.GetComponent<StyleCrate>())
            return;

        GainJuice(target.GetComponent<StyleCrate>().StyleOnKill, (int)Categories.Kill, "Kill");
        if (clock < MultiMaxTime)
            GainJuice(MultiGain, (int)Categories.Multikill, "Multikill");
        else if (clock < ComboMaxTime)
            GainJuice(ChainGain, (int)Categories.ChainKill, "Chain Kill");

        clock = 0f;
    }

    void OnDamage(int damage, Attack attack)
    {
        if (attack == null || attack.Agressor != GetComponent<Actor>())
            SpendJuice(damage * 4, (int)Categories.Hurt, "Hurt");
    }


    public void GainJuice(float gain, int category = -1, string text = "")
    {
        if (gain == 0)
            return;

        if (text != "")
            OnEvent?.Invoke(gain, category, text);

        SetJuiceLeft(Mathf.Clamp(JuiceLeft + gain * (player.IsGrounded ? 1 : 1.5f), 0f, JuiceMax));
    }


    public void SpendJuice(float cost, int category = -1, string text = "")
    {
        if (cost == 0)
            return;

        if (text != "")
            OnEvent?.Invoke(-cost, category, text);

        if (JuiceLeft - cost > 0f)
            SetJuiceLeft(JuiceLeft - cost);
        else
        {
            JuiceLeft = 0f;
            OnDeplete?.Invoke();
        }
    }


    void SetJuiceLeft(float juiceLeft)
    {
        JuiceLeft = juiceLeft;
        OnUpdate?.Invoke();

        bool lastCritical = Critical;
        Critical = juiceLeft > JuiceMax * CriticalPercent;
        if (lastCritical != Critical)
            OnCritical?.Invoke(Critical);
    }


    public float GetCriticalLevel()
    {
        return CriticalPercent * JuiceMax;
    }
}
