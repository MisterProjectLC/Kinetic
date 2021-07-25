using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int CurrentHealth = 5;
    public int MaxHealth = 5;

    [HideInInspector]
    public UnityAction OnDamage;
    [HideInInspector]
    public UnityAction OnHeal;
    [HideInInspector]
    public UnityAction OnDie;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }


    public void InflictDamage(int damage)
    {
        CurrentHealth -= damage;

        if (OnDamage != null)
            OnDamage.Invoke();
        if (CurrentHealth <= 0 && OnDie != null)
            OnDie.Invoke();
    }

    public void Heal(int heal)
    {
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + heal);
        if (OnHeal != null)
            OnHeal.Invoke();
    }
}