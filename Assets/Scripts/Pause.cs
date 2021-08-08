using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    PlayerInputHandler playerInput;
    [Tooltip("Shown when playing")]
    public GameObject[] PlayObjects;
    [Tooltip("Shown when paused")]
    public GameObject[] PausedObjects;

    private void Start()
    {
        playerInput = ActorsManager.Player.GetComponent<PlayerInputHandler>();
        StartCoroutine(InitialClose());
    }

    private void Update()
    {
        if (GetPause())
        {
            // Pause
            if (Time.timeScale != 0)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Time.timeScale = 0;
                playerInput.inputEnabled = false;
                foreach (GameObject GO in PausedObjects)
                    GO.SetActive(true);
                foreach (GameObject GO in PlayObjects)
                    GO.SetActive(false);
            }

            // Play
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                playerInput.inputEnabled = true;
                foreach (GameObject GO in PausedObjects)
                    GO.SetActive(false);
                foreach (GameObject GO in PlayObjects)
                    GO.SetActive(true);
            }
        }
    }

    IEnumerator InitialClose()
    {
        yield return new WaitForSeconds(0.01f);
        foreach (GameObject GO in PausedObjects)
            GO.SetActive(false);
        foreach (GameObject GO in PlayObjects)
            GO.SetActive(true);
    }

    public bool GetPause()
    {
        return Input.GetButtonDown(Constants.Pause);
    }
}
