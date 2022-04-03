using UnityEngine;

public abstract class IEnemyWeaponsManager : MonoBehaviour, SubcomponentUpdate
{
    [SerializeField]
    protected Weapon[] weapons;

    public Weapon[] GetWeapons()
    {
        return weapons;
    }

    public void OnUpdate()
    {
        ActivateWeapons();
    }

    abstract public void ActivateWeapons();
    
}