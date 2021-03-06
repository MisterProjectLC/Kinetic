using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [Header("References")]
    Projectile projectile;
    LineRenderer line;
    GameObject hookedObject;
    Vector3 relativePosition;

    [HideInInspector]
    public GrapplingHook Ability;

    [Header("Attributes")]
    public float PullForce = 10f;
    public float MaxDistance = 120f;

    PlayerCharacterController shooter;


    // Start is called before the first frame update
    void Awake()
    {
        projectile = GetComponent<Projectile>();
        projectile.OnHit += OnHit;
        line = GetComponent<LineRenderer>();
    }

    private void OnDisable()
    {
        if (Ability)
            Ability.OnHookDestroy();

        if (shooter)
            shooter.GravityEnabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, projectile.Shooter.transform.position);
        line.SetPosition(1, transform.position);

        if (projectile.KeepAlive)
        {
            if (!shooter)
                shooter = projectile.Shooter.GetComponent<PlayerCharacterController>();

            float multiplier = shooter.IsGrounded ? 5f : 1f;

            Vector3 force = (transform.position - shooter.GetPlayerCamera().transform.position).normalized;
            shooter.ReceiveForce(force * PullForce * multiplier * Time.deltaTime);

            if (!hookedObject || !hookedObject.activeInHierarchy)
                gameObject.SetActive(false);
            else
                transform.position = hookedObject.transform.position + relativePosition;
        }

        if ((projectile.Shooter.transform.position - transform.position).magnitude > MaxDistance)
            gameObject.SetActive(false);
    }


    void OnHit(Collider collider)
    {
        if (projectile.Stopped)
            return;

        hookedObject = collider.gameObject;
        relativePosition = transform.position - hookedObject.transform.position;
        projectile.KeepAlive = true;
        projectile.Stopped = true;
        GetComponent<AudioSource>().Play();
        //projectile.Shooter.GetComponent<PlayerCharacterController>().MoveVelocity = (transform.position -
        //        projectile.Shooter.GetComponent<PlayerCharacterController>().PlayerCamera.transform.position).normalized * PullForce/10;

        if (gameObject.activeInHierarchy)
            projectile.Shooter.GetComponent<PlayerCharacterController>().GravityEnabled = false;
    }
}
