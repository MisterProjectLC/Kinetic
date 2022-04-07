using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Powerup : MonoBehaviour
{
    static Vector3 rotation = new Vector3(0f, 60f, 0f);

    [SerializeField]
    GameObject icon;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.Self);
    }

    private void OnEnable()
    {
        icon.SetActive(true);
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacterController player = other.GetComponent<PlayerCharacterController>();

        if (!player || !ValidPowerup(player))
            return;

        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();

        ActivatePowerup(player.gameObject);

        StartCoroutine(AutoDestruct());
        
    }

    protected abstract void ActivatePowerup(GameObject player);

    protected virtual bool ValidPowerup(PlayerCharacterController player) {
        return true;
    }

    IEnumerator AutoDestruct()
    {
        icon.SetActive(false);

        if (GetComponent<AudioSource>())
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        else
            yield return new WaitForSeconds(0.001f);

        gameObject.SetActive(false);
    }
}
