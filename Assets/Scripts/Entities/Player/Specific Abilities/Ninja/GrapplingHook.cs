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

    public override void Execute(Input input)
    {
        hook.gameObject.SetActive(!hook.gameObject.activeInHierarchy);

        if (hook.gameObject.activeInHierarchy)
        {
            GetComponent<AudioSource>().Play();
            hook.transform.position = Mouth.position;
            hook.GetComponent<Projectile>().Setup(Mouth.forward, HitLayers, GetComponentInParent<Actor>().gameObject);
            ResetCooldown();
        } else
        {
            Timer = 0.5f;
        }
    }

    public void OnHookDestroy()
    {
        SetOffCooldown();
    }
}