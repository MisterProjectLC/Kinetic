using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverseer : MonoBehaviour
{
    public static UIOverseer UIO;

    [SerializeField]
    GameObject AmmoIndicator;

    // Start is called before the first frame update
    void Awake()
    {
        if (UIO)
            Destroy(UIO);

        UIO = this;
    }

    public GameObject GetAmmoIndicator()
    {
        return AmmoIndicator;
    }
}
