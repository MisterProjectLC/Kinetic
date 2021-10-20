using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCentibomb : MonoBehaviour
{
    [SerializeField]
    ChaseTarget CentibombLeft;
    [SerializeField]
    ChaseTarget CentibombRight;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<GameTrigger>().OnTriggerActivate += ActivateCentibombs;
    }

    void ActivateCentibombs()
    {
        if (CentibombLeft)
            CentibombLeft.enabled = true;

        if (CentibombRight)
            CentibombRight.enabled = true;
    }
}
