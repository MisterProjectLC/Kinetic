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
    public UnityAction<Health> OnCritical;
    [HideInInspector]
    public UnityAction<Attack, GameObject, bool> OnKill;
    [HideInInspector]
    public Actor Agressor;

    private void Awake()
    {
        Agressor = GetComponentInParent<Actor>();
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

    public void SetupClone(Attack clone)
    {
        //Debug.Log("Cloning from " + gameObject.name + " to " + clone.gameObject.name);

        clone.OnAttack += OnAttack;
        clone.OnCritical += OnCritical;
        clone.OnKill += OnKill;
    }
}
