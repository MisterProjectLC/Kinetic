using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Health HealthRef;
    private Actor actor;

    [Range (0, 3f)]
    public float DamageSensitivity = 1f;


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
        HealthRef.InflictDamage((int)Mathf.Floor(damage*DamageSensitivity));
    }

    public void InflictDamage(int damage, Attack attack)
    {
        HealthRef.OnDie += attack.OnKill;
        Debug.Log("Entered OnDie");
        HealthRef.InflictDamage((int)Mathf.Floor(damage * DamageSensitivity));
        HealthRef.OnDie -= attack.OnKill;
        Debug.Log("Left OnDie");
    }
}
