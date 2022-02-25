using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    int damage = 1;
    int actualDamage = 1;

    public int Damage
    {
        get { return actualDamage; }
        set { actualDamage = value; }
    }

    public UnityAction<GameObject, float, int> OnAttack;
    public UnityAction<Health> OnCritical;
    public UnityAction<Attack, GameObject, bool> OnKill;
    [HideInInspector]
    public Actor Agressor;

    bool poolable = false;

    private void Awake()
    {
        actualDamage = damage;
        Agressor = GetComponentInParent<Actor>();
        poolable = !(GetComponent<Poolable>() == null);
    }

    private void OnDisable()
    {
        if (poolable)
        {
            actualDamage = damage;
            OnAttack = null;
            OnCritical = null;
            OnKill = null;
        }
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
            targetHit.InflictDamage((int)(multiplier* actualDamage), this);
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
