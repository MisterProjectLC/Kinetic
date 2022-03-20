using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Description : MonoBehaviour
{
    [TextArea(3, 10)]
    public string description = "";

    public LocalizedString Localized;


    public string Value
    {
        get
        {
            return Localized.value;
        }
    }
}
