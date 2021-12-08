using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrystalShield : MonoBehaviour
{
    [SerializeField]
    List<Crystal> generators;
    int generatorCount = 0;

    [SerializeField]
    GameObject shieldedObject;
    List<GameObject> Holograms = new List<GameObject>();

    public UnityAction OnDeactivate;


    private void Start()
    {
        foreach (Crystal generator in generators)
        {
            generator.Setup(this);
            RegisterGenerator();
        }
    }

    private void OnEnable()
    {
        foreach (MeshRenderer MR in shieldedObject.transform.GetComponentsInChildren<MeshRenderer>())
            if (MR.gameObject.name == "Shield Hologram")
                Holograms.Add(MR.gameObject);


        foreach (Damageable damageable in shieldedObject.GetComponentsInChildren<Damageable>())
            damageable.DamageSensitivity = 0f;
    }

    private void OnDisable()
    {
        foreach (Damageable damageable in shieldedObject.GetComponentsInChildren<Damageable>())
            damageable.DamageSensitivity = 1f;
    }


    public void ReactivateEverything()
    {
        generatorCount = generators.Count;
        foreach (Crystal generator in generators)
            generator.Activate();


        foreach (GameObject GO in Holograms)
            GO.SetActive(true);
        gameObject.SetActive(true);
    }

    void RegisterGenerator()
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
            OnDeactivate?.Invoke();
            foreach (GameObject GO in Holograms)
                GO.SetActive(false);
            gameObject.SetActive(false);
        }

    }
}
