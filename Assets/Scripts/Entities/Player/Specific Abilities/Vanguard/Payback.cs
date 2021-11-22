using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payback : Ability
{
    [Tooltip("References")]
    [SerializeField]
    GameObject RedShield;
    [SerializeField]
    GameObject Explosion;
    Attack attack;

    [Tooltip("Attributes")]
    [SerializeField]
    float Knockback = 200f;
    [SerializeField]
    float Duration = 4f;
    [SerializeField]
    int MaxDamageAbsorbed = 12;

    int damagedSummation = 0;
    bool absorbing = false;
    float clock = 0f;

    [Tooltip("Sounds")]
    [SerializeField]
    AudioClip ActivateSound;
    [SerializeField]
    AudioClip DetonateSound;


    private void Start()
    {
        attack = GetComponent<Attack>();

        foreach (Attack attack in GetComponentInParent<PlayerCharacterController>().GetComponentsInChildren<Attack>())
        {
            attack.OnAttack += OnAttack;
        }
        GetComponentInParent<Damageable>().OnRawDamage += OnDamage;
    }

    public override void Execute(Input input)
    {
        ResetCooldown();
        // Activate sooner
        if (absorbing)
            absorbing = false;
        else
        {
            GetComponentInParent<Damageable>().DamageSensitivity = 0f;
            RedShield.SetActive(true);
            absorbing = true;
            damagedSummation = 0;
            clock = 0f;
            PlaySound(ActivateSound);
            StartCoroutine("ActivatePayback");
        }
       
    }

    void OnDamage(int damage)
    {
        if (!absorbing)
            return;

        damagedSummation += damage;
        if (damagedSummation > MaxDamageAbsorbed)
            absorbing = false;
    }

    void OnAttack(GameObject attack, float multiplier, int damage)
    {
        OnDamage(damage);
    }

    IEnumerator ActivatePayback()
    {
        while (absorbing)
        {
            yield return new WaitForSeconds(0.1f);
            clock += 0.1f;
            if (clock > Duration)
                absorbing = false;
        }
        GetComponentInParent<Damageable>().DamageSensitivity = 1f;
        RedShield.SetActive(false);
        if (damagedSummation > attack.Damage/2)
            PlaySound(DetonateSound);

        GameObject newObject = ObjectManager.OM.SpawnObjectFromPool(Explosion.GetComponent<Poolable>().Type, Explosion);
        newObject.transform.position = transform.position;

        attack.SetupClone(newObject.GetComponent<Attack>());
        newObject.GetComponent<Attack>().Damage = Mathf.Clamp(damagedSummation, 0, attack.Damage);
        newObject.GetComponent<StatusEffectApplier>().Knockback = Knockback * Mathf.Clamp01(damagedSummation / (float)MaxDamageAbsorbed);
        Debug.Log("Summation: " + damagedSummation);

        SetOffCooldown();
    }
}
