using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalShield : MonoBehaviour
{
    [SerializeField]
    GameObject shieldedObject;
    int generatorCount = 0;

    private void OnEnable()
    {
        foreach (Damageable damageable in shieldedObject.GetComponentsInChildren<Damageable>())
            damageable.DamageSensitivity = 0f;
    }

    private void OnDisable()
    {
        foreach (Damageable damageable in shieldedObject.GetComponentsInChildren<Damageable>())
            damageable.DamageSensitivity = 1f;
    }


    // Update is called once per frame
    public void RegisterGenerator()
    {
        generatorCount++;
        if (generatorCount > 0)
            gameObject.SetActive(true);
    }

    public void DisableGenerator()
    {
        generatorCount--;
        if (generatorCount <= 0)
            gameObject.SetActive(false);
    }
}
