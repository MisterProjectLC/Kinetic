using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowHealthWarning : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GetComponentInParent<Health>().OnCriticalLevel += PlayCritical;

    }

    void PlayCritical()
    {
        GetComponent<AudioSource>().Play();
        GetComponentInParent<Health>().OnCriticalLevel -= PlayCritical;
    }
}
