using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    static float LastAttackWait = 20f;

    [SerializeField]
    IntReference currentHealth;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth.Value = value;
        }
    }
    public int CriticalHealth = 1;

    [SerializeField]
    IntReference maxHealth;
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth.Value = value;
        }
    }

    bool critical = false;
    bool died = false;

    [HideInInspector]
    public Attack lastAttack = null;
    UnityAction<Attack, GameObject, bool> lastAttackOnKill;
    float lastAttackTime = 0f;

    [HideInInspector]
    public UnityAction<int> OnDamage;
    [HideInInspector]
    public UnityAction<int, Attack> OnDamageAttack;
    [HideInInspector]
    public UnityAction OnCriticalLevel;
    [HideInInspector]
    public UnityAction<int> OnHeal;
    [HideInInspector]
    public UnityAction OnDie;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
    }


    public void InflictDamage(int damage)
    {
        InflictDamage(damage, null);
    }


    public void InflictDamage(int damage, Attack source)
    {
        if (died)
            return;

        currentHealth.Value -= damage;
        OnDamage?.Invoke(damage);
        OnDamageAttack?.Invoke(damage, source);

        lastAttack = source;
        lastAttackOnKill = null;
        lastAttackOnKill += source.OnKill;
        lastAttackTime = Time.time;

        if (currentHealth.Value <= CriticalHealth && !critical)
        {
            critical = true;
            source.OnCritical?.Invoke(this);
            OnCriticalLevel?.Invoke();
        }

        if (currentHealth.Value <= 0)
            Die();
    }


    void Die()
    {
        if (died)
            return;

        died = true;
        OnDie += () => lastAttackOnKill?.Invoke(lastAttack, gameObject, false);
        OnDie?.Invoke();
    }

    public void Kill()
    {
        Kill(true);
    }

    public void Kill(bool environmental)
    {
        if (died)
            return;

        if (environmental && lastAttackTime + LastAttackWait > Time.time)
        {
            Debug.Log("Environmental Kill");
            lastAttackOnKill?.Invoke(lastAttack, gameObject, true);
        }

        Die();
    }

    public void Heal(int heal)
    {
        currentHealth.Value = Mathf.Min(maxHealth, currentHealth + heal);
        OnHeal?.Invoke(heal);

        if (currentHealth > CriticalHealth && critical)    
            critical = false;
    }
}