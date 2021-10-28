using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static Pause Ps;


    public static bool Paused { get; private set; } = false;
    const string PauseB = "Pause";

    PlayerInputHandler playerInput;
    [Tooltip("Shown when playing")]
    public GameObject[] PlayObjects;
    [Tooltip("Shown when paused")]
    public GameObject[] PausedObjects;

    float oldTimeScale = 1f;


    private void Awake()
    {
        if (Ps)
            Destroy(gameObject);
        else
            Ps = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = ActorsManager.AM.GetPlayer().GetComponent<PlayerInputHandler>();
        StartCoroutine(InitialClose());
    }

    private void Update()
    {
        if (GetPause())
            TogglePause();
    }


    public void TogglePause()
    {
        Paused = !Paused;
        playerInput.inputEnabled = !Paused;
        Cursor.lockState = Paused ? CursorLockMode.Confined : CursorLockMode.Locked;
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
