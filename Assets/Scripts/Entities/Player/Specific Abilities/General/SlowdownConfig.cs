using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowdownConfig", menuName = "ScriptableObjects/SlowdownConfig")]
public class SlowdownConfig : ScriptableObject
{
    public float CriticalPercent = 0.8f;
    public float DepleteRate = 0.25f;
    public float DamageLoss = 2.5f;

    public float MultiMaxTime = 0.25f;
    public float ComboMaxTime = 3f;
    public float CombatMaxTime = 6f;
    public float MovementSpeed = 25f;

    [Tooltip("Gain amounts")]
    public float ChainGain = 1.5f;
    public float MultiGain = 2f;
    public float IndirectGain = 5f;
    public float VarietyGain = 1f;
    public float MovementGain = 0.5f;
}
