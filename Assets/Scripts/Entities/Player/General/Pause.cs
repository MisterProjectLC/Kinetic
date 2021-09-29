using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool Paused { get; private set; } = false;
    const string PauseB = "Pause";

    PlayerInputHandler playerInput;
    [Tooltip("Shown when playing")]
    public GameObject[] PlayObjects;
    [Tooltip("Shown when paused")]
    public GameObject[] PausedObjects;

    float oldTimeScale = 1f;

    private void Start()
    {
        StartCoroutine(InitialClose());
    }

    private void Update()
    {
        if (GetPause())
        {
            Paused = !Paused;
            // Pause
            if (Paused)
            {
                Cursor.lockState = CursorLockMode.Confined;
                oldTimeScale = Time.timeScale;
                Time.timeScale = 0;
                foreach (GameObject GO in PausedObjects)
                    GO.SetActive(true);
                foreach (GameObject GO in PlayObjects)
                    GO.SetActive(false);
            }

            // Play
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = oldTimeScale;
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
        return Input.GetButtonDown(PauseB);
    }
}
