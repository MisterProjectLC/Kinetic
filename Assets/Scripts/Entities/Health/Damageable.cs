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
        OnRawDamage?.Invoke(damage);
        OnDamage?.Invoke(damageInflicted);
        HealthRef.InflictDamage(damageInflicted, attack);
    }


    public Health GetHealth()
    {
        return HealthRef;
    }
}
