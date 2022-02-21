using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [Header("Attributes")]
    public int Damage = 1;

    public UnityAction<GameObject, float, int> OnAttack;
    public UnityAction<Health> OnCritical;
    public UnityAction<Attack, GameObject, bool> OnKill;
    [HideInInspector]
    public Actor Agressor;

    private void Awake()
    {
        Agressor = GetComponentInParent<Actor>();
    }

    private void OnDisable()
    {
        OnAttack = null;
        OnCritical = null;
        OnKill = null;
    }

    public void AttackTarget(GameObject target, float multiplier = 1f)
    {
        Damageable targetHit = target.GetComponent<Damageable>();

        void OnDamageInvoke(int damage)
        {
            OnAttack?.Invoke(target, multiplier* targetHit.DamageSensitivity, damage);
        }

        // Damage
        if (targetHit)
        {
            targetHit.OnDamage += OnDamageInvoke;
            targetHit.InflictDamage((int)(multiplier*Damage), this);
            targetHit.OnDamage -= OnDamageInvoke;
        }
    }


    public void SetupClone(Attack clone)
    {
        //Debug.Log("Cloning from " + gameObject.name + " to " + clone.gameObject.name);

        clone.OnAttack += OnAttack;
        clone.OnCritical += OnCritical;
        clone.OnKill += OnKill;
    }
}
