using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspendedCrate : MonoBehaviour
{
    [SerializeField]
    Vector3 MoveDirection;

    // Update is called once per frame
    void Update()
    {
        transform.position += MoveDirection * Time.deltaTime;
    }
}
