using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class Powerup : MonoBehaviour
{
    static Vector3 rotation = new Vector3(0f, 60f, 0f);

    public UnityAction<GameObject> OnPowerup;

    [SerializeField]
    GameObject icon;

    private void Start()
    {
        Debug.Log("Tywin " + gameObject.name);

        Setup();
    }

    protected abstract void Setup();

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.Self);
    }

    private void OnEnable()
    {
        Debug.Log(gameObject.name);
        icon.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacterController player = other.GetComponent<PlayerCharacterController>();

        if (!player)
            return;

        if (!ValidPowerup(player))
            return;

        if (GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().Play();
            Debug.Log("Played sound");
        }

        OnPowerup?.Invoke(player.gameObject);

        StartCoroutine(AutoDestruct());
        
    }

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
