using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Pickup : MonoBehaviour
{
    UnityAction OnPickup;
    public void SubscribeToPickup(UnityAction subscriber) { OnPickup += subscriber; }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacterController player = other.GetComponent<PlayerCharacterController>();

        if (!player || !ValidPowerup(player))
            return;

        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();

        OnPickup?.Invoke();
        ActivatePowerup(player.gameObject);

        StartCoroutine(AutoDestruct());

    }

    protected abstract void ActivatePowerup(GameObject player);

    protected virtual bool ValidPowerup(PlayerCharacterController player)
    {
        return true;
    }

    IEnumerator AutoDestruct()
    {
        OnAutoDestruct();

        if (GetComponent<AudioSource>())
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        else
            yield return new WaitForSeconds(0.001f);

        gameObject.SetActive(false);
    }

    protected virtual void OnAutoDestruct()
    {

    }
}
