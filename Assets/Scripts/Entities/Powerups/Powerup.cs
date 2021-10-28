using UnityEngine;
using UnityEngine.Events;

public class Powerup : MonoBehaviour
{
    static Vector3 rotation = new Vector3(0f, 60f, 0f);

    public UnityAction<GameObject> OnPowerup;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacterController>())
        {
            if (OnPowerup != null)
                OnPowerup.Invoke(other.gameObject);

            Destroy(gameObject);
        }
    }
}
