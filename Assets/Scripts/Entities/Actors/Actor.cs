using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Tooltip("Actors with the same Affiliation are considered friendly")]
    public int Affiliation;

    public void Start()
    {
        if (ActorsManager.AM)
            ActorsManager.AM.RegisterActor(this);
    }
}
