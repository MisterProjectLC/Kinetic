using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Health HealthRef;
    private Actor actor;

    [Range (0, 3f)]
    public float DamageSensitivity = 1f;
    Dictionary<string, float> Modifiers = null;

    public UnityAction<int> OnRawDamage;
    public UnityAction<int> OnDamage;


    private void Start()
    {
        actor = HealthRef.GetComponent<Actor>();
    }

    public int? GetAffiliation()
    {
        return actor ? actor.Affiliation : (int?)null;
    }

    public void InflictDamage(int damage)
    {
        InflictDamage(damage, null);
    }

    public void InflictDamage(int damage, Attack attack)
    {
        int damageInflicted = (int)Mathf.Floor(damage * DamageSensitivity);
        float multiplier = 1f;
        if (Modifiers != null)
        {
            foreach (float modifier in Modifiers.Values)
                multiplier *= modifier;
            damageInflicted = (int)Mathf.Floor(damageInflicted * multiplier);
        }


        OnRawDamage?.Invoke(damage);
        OnDamage?.Invoke(damageInflicted);
        HealthRef.InflictDamage(damageInflicted, attack);
    }


    public Health GetHealth()
    {
        return HealthRef;
    }


    public void ApplyModifier(string name, float value)
    {
        if (Modifiers == null)
            Modifiers = new Dictionary<string, float>(3);

        if (Modifiers.ContainsKey(name))
            Modifiers.Remove(name);
        Modifiers.Add(name, value);
    }
}
