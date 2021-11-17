using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    static float LastAttackWait = 15f;

    [HideInInspector]
    public int CurrentHealth = 5;
    public int CriticalHealth = 1;
    public int MaxHealth = 5;
    bool critical = false;
    bool died = false;

    [HideInInspector]
    public Attack lastAttack = null;
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
    void Start()
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

        CurrentHealth -= damage;
        OnDamage?.Invoke(damage);
        OnDamageAttack?.Invoke(damage, source);
        lastAttack = source;
        lastAttackTime = Time.fixedTime;

        if (CurrentHealth <= CriticalHealth && !critical)
        {
            critical = true;
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
        if (lastAttack)
            OnDie += () => lastAttack.OnKillTarget?.Invoke(gameObject);
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

        if (environmental && ReceivedRecentPlayerAttack())
            lastAttack.OnIndirectKill?.Invoke();

        Die();
    }

    public void Heal(int heal)
    {
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + heal);
        OnHeal?.Invoke(heal);

        if (CurrentHealth > CriticalHealth && critical)    
            critical = false;
    }

    public bool ReceivedRecentPlayerAttack()
    {
        return lastAttackTime + LastAttackWait > Time.fixedTime ? (lastAttack ? lastAttack.Agressor == ActorsManager.AM.GetPlayer() : false) : false;
    }
}