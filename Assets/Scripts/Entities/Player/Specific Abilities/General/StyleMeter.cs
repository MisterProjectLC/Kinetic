using UnityEngine;
using UnityEngine.Events;

public class StyleMeter : MonoBehaviour
{
    Health health;
    Ability lastAbility = null;

    public UnityAction OnUpdate;
    public UnityAction OnDeplete;
    public UnityAction<float, string> OnEvent;

    public float JuiceLeft { get; private set; }

    [SerializeField]
    public float JuiceMax { get; private set; } = 10f;

    [SerializeField]
    float DepleteRate = 0.25f;

    [SerializeField]
    float ComboMaxTime = 3f;
    float clock = 0f;

    private void Start()
    {
        SetJuiceLeft(JuiceMax);
        health = GetComponentInParent<Health>();
        health.OnDamage += OnDamage;
        foreach (Attack attack in GetComponentInParent<PlayerCharacterController>().GetComponentsInChildren<Attack>())
        {
            attack.OnKill += StyleKill;
        }

        foreach (Ability ability in GetComponentInParent<PlayerCharacterController>().GetComponentsInChildren<Ability>())
        {
            ability.OnExecuteAbility += AbilityUsage;
        }
    }

    private void Update()
    {
        if (JuiceLeft > 0f)
            SpendJuice(Time.deltaTime * DepleteRate);

        if (clock < ComboMaxTime)
            clock += Time.deltaTime;
    }

    void AbilityUsage(Ability ability)
    {
        if (lastAbility != ability)
        {
            lastAbility = ability;
            GainJuice(0.25f);
            if (clock < ComboMaxTime)
                GainJuice(0.75f, "Variety");
        }
    }

    void StyleKill()
    {
        if (clock < ComboMaxTime)
            GainJuice(2f, "Chain Kill");
        else
            GainJuice(1f, "Kill");
        clock = 0f;
    }

    void OnDamage(int damage)
    {
        SpendJuice(damage * 4, "Damage");
        if (JuiceLeft > 0f)
            health.Heal(damage);
    }


    public void GainJuice(float gain, string text = "")
    {
        if (text != "")
            OnEvent?.Invoke(gain, text);

        SetJuiceLeft(Mathf.Clamp(JuiceLeft + gain, 0f, JuiceMax));
    }


    public void SpendJuice(float cost, string text = "")
    {
        if (text != "")
            OnEvent?.Invoke(-cost, text);

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
    }
}
