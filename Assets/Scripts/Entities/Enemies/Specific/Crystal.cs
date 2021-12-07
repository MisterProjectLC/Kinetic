using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField]
    CrystalShield GeneratedShield;

    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        if (GeneratedShield)
            GeneratedShield.RegisterGenerator();
        GetComponent<Health>().OnDie += GeneratedShield.DisableGenerator;
    }

    private void Update()
    {
        if (GeneratedShield)
            lineRenderer.SetPosition(1, GeneratedShield.transform.position);
    }
}
