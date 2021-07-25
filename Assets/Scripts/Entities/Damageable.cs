using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Health HealthRef;
    private Actor actor;

    [Range (0, 1f)]
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
        Debug.Log(gameObject.name);
        HealthRef.InflictDamage((int)Mathf.Floor(damage*DamageSensitivity));
    }
}
