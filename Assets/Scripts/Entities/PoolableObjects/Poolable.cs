using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    [SerializeField]
    public PoolableEnum Type;

    [HideInInspector]
    public bool AlreadyInitialized = false;
}
