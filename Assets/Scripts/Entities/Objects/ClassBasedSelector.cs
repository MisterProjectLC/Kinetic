using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassBasedSelector : MonoBehaviour
{
    [SerializeField]
    SerializableDictionary<Hermes.PlayerClass, GameObject> ObjectDictionary;

    // Start is called before the first frame update
    void Start()
    {
        if (ObjectDictionary.ContainsKey(Hermes.CurrentClass))
        {
            ObjectDictionary[Hermes.CurrentClass].SetActive(true);
        }
    }

}
