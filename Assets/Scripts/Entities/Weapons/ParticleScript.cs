using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particles.playOnAwake = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!particles.isPlaying)
            ObjectManager.OM.EraseObject(GetComponent<Poolable>());
    }
}
