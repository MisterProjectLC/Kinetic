using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyShield : MonoBehaviour
{
    [SerializeField]
    protected GameObject shieldedObject;
    protected List<GameObject> Holograms = new List<GameObject>();
    public UnityAction OnDeactivate;

    protected void Start()
    {
        foreach (MeshRenderer MR in shieldedObject.transform.GetComponentsInChildren<MeshRenderer>())
            if (MR.gameObject.name == "Shield Hologram")
                Holograms.Add(MR.gameObject);

        SetShield(true);
    }


    public void SetShield(bool activated)
    {
        if (!activated)
            OnDeactivate?.Invoke();
        foreach (GameObject GO in Holograms)
            GO.SetActive(activated);

        foreach (Damageable damageable in shieldedObject.GetComponentsInChildren<Damageable>())
            if (!damageable.gameObject.name.Contains("IgnoreShield"))
                damageable.DamageSensitivity = activated ? 0f : 1f;
    }
}
