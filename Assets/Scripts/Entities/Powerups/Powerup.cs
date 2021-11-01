using System.Collections;
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
            if (GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().Play();
                Debug.Log("Played sound");
            }

            if (OnPowerup != null)
                OnPowerup.Invoke(other.gameObject);

            StartCoroutine("AutoDestruct");
        }
    }

    IEnumerator AutoDestruct()
    {
        if (GetComponent<AudioSource>())
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        else
            yield return new WaitForSeconds(0.001f);

        Destroy(gameObject);
    }
}
