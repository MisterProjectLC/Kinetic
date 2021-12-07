using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalShield : MonoBehaviour
{
    [SerializeField]
    GameObject shieldedObject;
    List<GameObject> Holograms = new List<GameObject>();

    int generatorCount = 0;

    private void OnEnable()
    {
        foreach (MeshRenderer MR in shieldedObject.transform.GetComponentsInChildren<MeshRenderer>())
            if (MR.gameObject.name == "Shield Hologram" || MR.gameObject.name == "Cannon")
                Holograms.Add(MR.gameObject);


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
        {
            foreach (GameObject GO in Holograms)
                GO.SetActive(true);
            gameObject.SetActive(true);
        }
    }

    public void DisableGenerator()
    {
        generatorCount--;
        if (generatorCount <= 0)
        {
            foreach (GameObject GO in Holograms)
                GO.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
