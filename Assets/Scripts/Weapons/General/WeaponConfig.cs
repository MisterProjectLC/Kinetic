using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [Header("Attributes")]
    [Tooltip("Pushback force when firing the gun")]
    [SerializeField]
    private float backwardsForce = 10f;

    [Tooltip("Max angle at which the bullets spread from the center")]
    [Range(0f, 10f)]
    [SerializeField]
    private float MaxSpreadAngle = 0f;

    [Tooltip("Amount of bullets sent")]
    [SerializeField]
    private int BulletCount = 12;

    [Tooltip("If enabled, holding the button engages repeated shooting")]
    public bool Automatic = false;
    public float FireCooldown = 0f;

    public float InitialFireCooldown = 0f;
}

