using UnityEngine;
using UnityEngine.Events;

public class GameTrigger : MonoBehaviour
{
    public UnityAction OnTriggerActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (OnTriggerActivate != null)
            OnTriggerActivate.Invoke();
    }
}
