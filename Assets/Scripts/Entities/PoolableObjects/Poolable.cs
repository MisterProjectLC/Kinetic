using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    [SerializeField]
    public ObjectManager.PoolableType Type;

    [HideInInspector]
    public bool alreadyInitialized = false;
}
