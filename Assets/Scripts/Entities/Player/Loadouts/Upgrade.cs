using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public LocalizedString LocalizedName;
    [HideInInspector]
    public string Type;
}
