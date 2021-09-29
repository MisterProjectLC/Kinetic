using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    private Health m_Health;

    // Start is called before the first frame update
    void Start()
    {
        m_Health = GetComponent<Health>();
        m_Health.OnDie += OnDie;
    }

    void OnDie()
    {
        if (GetComponent<Poolable>())
            ObjectManager.OM.EraseObject(GetComponent<Poolable>());
        else
            Destroy(gameObject);
    }
}
