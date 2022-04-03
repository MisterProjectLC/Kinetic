using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEnemyWeaponsManager : MonoBehaviour
{
    [SerializeField]
    protected Weapon[] weapons;

    public Weapon[] GetWeapons()
    {
        return weapons;
    }
    abstract public void ActivateWeapons();
    
}