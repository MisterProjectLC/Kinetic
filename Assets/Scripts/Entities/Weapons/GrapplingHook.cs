using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : Ability
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
        {
            attack.OnKill += ResetCooldown;
        }
    }

    public override void Execute()
    {
        hook.gameObject.SetActive(!hook.gameObject.activeInHierarchy);

        if (hook.gameObject.activeInHierarchy)
        {
            hook.transform.position = Mouth.position;
            hook.GetComponent<Projectile>().Setup(Mouth.forward, HitLayers, GetComponentInParent<Actor>().gameObject);
            ResetCooldown();
        }
    }

    public void OnHookDestroy()
    {
        SetOffCooldown();
    }
}
