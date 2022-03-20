using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleCrate : MonoBehaviour
{
    public enum EnemyType
    {
        Minion,
        BigMinion,
        Knight,
        Boss
    }

    public float StylePerDamage = 0f;
    public float StyleOnCritical = 0f;
    public float StyleOnKill = 0.5f;
    public EnemyType enemyType = EnemyType.Minion;
}
