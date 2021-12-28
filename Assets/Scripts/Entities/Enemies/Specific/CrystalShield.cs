using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrystalShield : EnemyShield
{
    [SerializeField]
    List<Crystal> generators;
    int generatorCount = 0;

    new void Start()
    {
        foreach (Crystal generator in generators)
        {
            generator.Setup(this);
            RegisterGenerator();
        }

        base.Start();
    }

    public void ReactivateEverything()
    {
        foreach (Crystal generator in generators)
            generator.Activate();

        generatorCount = generators.Count;
        SetShield(true);
    }

    void RegisterGenerator()
    {
        generatorCount++;
        if (generatorCount > 0)
            SetShield(true);
    }

    public void DisableGenerator()
    {
        generatorCount--;
        if (generatorCount <= 0)
            SetShield(false);
    }
}
