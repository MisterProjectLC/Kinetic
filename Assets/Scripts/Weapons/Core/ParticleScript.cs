using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    private ParticleSystem[] particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles)
            ps.playOnAwake = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool isPlaying = false;
        foreach (ParticleSystem ps in particles)
            if (ps.isPlaying)
            {
                isPlaying = true;
                break;
            }

        if (!isPlaying)
            ObjectManager.EraseObject(GetComponent<Poolable>());
    }
}
