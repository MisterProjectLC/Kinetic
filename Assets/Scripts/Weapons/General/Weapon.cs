using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Where the bullets exit from")]
    public Transform Mouth;

    [Header("General")]
    public string DisplayName = "";

    [Tooltip("Layer mask")]
    public LayersConfig HitLayers;

    [Header("Attributes")]
    [Tooltip("Pushback force when firing the gun")]
    [SerializeField]
    private float backwardsForce = 10f;
    public float BackwardsForce { get { return backwardsForce; } }

    [Tooltip("Max angle at which the bullets spread from the center")]
    [Range(0f, 10f)]
    [SerializeField]
    protected float MaxSpreadAngle = 0f;

    [Tooltip("Amount of bullets sent")]
    [SerializeField]
    protected int BulletCount = 1;

    [Tooltip("If enabled, holding the button engages repeated shooting")]
    public bool Automatic = false;
    public float FireCooldown = 0f;
    float clock = 0f;

    public float InitialFireCooldown = 0f;

    [SerializeField]
    AudioClip[] SoundEffects;

    public UnityAction OnFire;

    private void Start()
    {
        clock = InitialFireCooldown;
        MaxSpreadAngle /= 100f;
    }

    private void Update()
    {
        if (clock > 0f)
            clock -= Time.deltaTime;
    }

    public void Trigger()
    {
        if (clock <= 0f)
        {
            Fire();
            clock = FireCooldown;
        }
    }

    public void Fire()
    {
        OnFire?.Invoke();
        if (SoundEffects.Length > 0)
            PlaySound(SoundEffects[Random.Range(0, SoundEffects.Length)]);

        for (int i = 0; i < BulletCount; i++)
        {
            // Get Direction
            float spread = Random.Range(0f, MaxSpreadAngle);
            Vector3 direction = Vector3.Slerp(Mouth.forward, Random.insideUnitSphere, spread);
            Shoot(direction);
            
        }
    }

    public void ResetClock()
    {
        clock = 0f;
    }

    protected void PlaySound(AudioClip sound)
    {
        GetComponent<AudioSource>().clip = sound;
        GetComponent<AudioSource>().Play();
    }

    public abstract void Shoot(Vector3 direction);
}
