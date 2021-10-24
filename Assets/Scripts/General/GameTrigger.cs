using UnityEngine;
using UnityEngine.Events;

public class GameTrigger : MonoBehaviour
{
    public UnityAction OnTriggerActivate;

    private void Awake()
    {
        if (GetComponent<AudioSource>())
            OnTriggerActivate += GetComponent<AudioSource>().Play;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnTriggerActivate != null)
            OnTriggerActivate.Invoke();
    }
}
