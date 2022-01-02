using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : SecondaryAbility
{
    [SerializeField]
    private GameObject hookPrefab;
    public Transform Mouth;
    public LayerMask HitLayers;

    Hook hook;

    private void Start()
    {
        hook = Instantiate(hookPrefab, transform.position, Quaternion.identity).GetComponent<Hook>();
        hook.Ability = this;
        hook.gameObject.SetActive(false);

        foreach (Attack attack in GetComponentInParent<PlayerCharacterController>().GetComponentsInChildren<Attack>())
            attack.OnKill += (Attack a, GameObject g, bool b) => ResetCooldown();

        GetComponentInParent<StyleMeter>().OnCritical += (critical) => { if (critical) ResetCooldown(); };
    }

    public override void Execute(Input input)
    {
        hook.gameObject.SetActive(!hook.gameObject.activeInHierarchy);

        if (hook.gameObject.activeInHierarchy)
        {
            GetComponent<AudioSource>().Play();
            hook.transform.position = Mouth.position;
            hook.GetComponent<Projectile>().Setup(Mouth.forward, HitLayers, GetComponentInParent<Actor>().gameObject);
            GetComponentInParent<PlayerCharacterController>().GravityEnabled = true;
            ResetCooldown();
        } else
        {
            if (GetComponentInParent<StyleMeter>().Critical)
                ResetCooldown();
        }
    }

    public void OnHookDestroy()
    {
        GetComponentInParent<PlayerCharacterController>().GravityEnabled = true;
        if (!GetComponentInParent<StyleMeter>().Critical)
            SetOffCooldown();
    }
}
