using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<GameTrigger>().Activate();
    }
}
