using UnityEngine;
using UnityEngine.Events;
using SmartData.SmartInt;

public class Health : MonoBehaviour
{
    static float LastAttackWait = 20f;

    public IntWriter CurrentHealth;
    public int CriticalHealth = 1;
    public IntWriter MaxHealth;

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
        CurrentHealth.value = MaxHealth;
    }


    public void InflictDamage(int damage)
    {
        InflictDamage(damage, null);
    }


    public void InflictDamage(int damage, Attack source)
    {
        if (died)
            return;

        CurrentHealth.value -= damage;
        OnDamage?.Invoke(damage);
        OnDamageAttack?.Invoke(damage, source);

        lastAttack = source;
        lastAttackOnKill = null;
        lastAttackOnKill += source.OnKill;
        lastAttackTime = Time.time;

        if (CurrentHealth <= CriticalHealth && !critical)
        {
            critical = true;
            source.OnCritical?.Invoke(this);
            OnCriticalLevel?.Invoke();
        }

        if (CurrentHealth <= 0)
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
        CurrentHealth.value = Mathf.Min(MaxHealth, CurrentHealth + heal);
        OnHeal?.Invoke(heal);

        if (CurrentHealth > CriticalHealth && critical)    
            critical = false;
    }
}