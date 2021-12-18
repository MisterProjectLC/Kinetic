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

        if (projectile.Shooter)
            projectile.Shooter.GetComponent<PlayerCharacterController>().GravityEnabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, projectile.Shooter.transform.position);
        line.SetPosition(1, transform.position);

        if (projectile.KeepAlive)
        {
            projectile.Shooter.GetComponent<PlayerCharacterController>().ApplyForce((transform.position - 
                projectile.Shooter.GetComponent<PlayerCharacterController>().PlayerCamera.transform.position).normalized
                * PullForce * Time.deltaTime);

            if (!hookedObject || !hookedObject.activeInHierarchy)
                gameObject.SetActive(false);
            else
                transform.position = hookedObject.transform.position + relativePosition;
        }
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
