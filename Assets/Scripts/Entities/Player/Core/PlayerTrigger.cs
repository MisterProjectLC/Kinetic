using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    PlayerCharacterController player;
    UnityAction<Collider> OnTrigger;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        OnTrigger += player.OnTrigger;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(other);
    }
}
