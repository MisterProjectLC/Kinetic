using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [Header("Attributes")]
    public int Damage = 1;

    [HideInInspector]
    public UnityAction<GameObject, float, int> OnAttack;
    [HideInInspector]
    public UnityAction OnKill;
    [HideInInspector]
    public UnityAction<Attack> OnKillAttack;
    [HideInInspector]
    public Actor Agressor;

    private void Awake()
    {
        Agressor = GetComponentInParent<Actor>();
    }

    private void Start()
    {
        OnKill += Kill;
    }

    void Kill()
    {
        OnKillAttack?.Invoke(this);
    }


    public void AttackTarget(GameObject target, float multiplier = 1f)
    {
        // Damage
        if (target.GetComponent<Damageable>())
        {
            OnAttack?.Invoke(target, multiplier, (int)(multiplier * Damage));
            target.GetComponent<Damageable>().InflictDamage((int)(multiplier*Damage), this);
        }
    }
}
