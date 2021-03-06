using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public enum PausedMenus {
        LoadoutMenu,
        PauseMenu
    }

    public static Pause Ps;

    [SerializeField]
    bool CinematicMode = false;

    [SerializeField]
    bool TrailerMode = false;

    public static bool Paused { get; private set; } = false;
    const string PauseB = "Pause";

    PlayerInputHandler playerInput;
    [Tooltip("Shown when playing")]
    public GameObject[] PlayObjects;

    [Tooltip("Shown when playing in trailer")]
    public GameObject[] TrailerObjects;

    [Tooltip("Shown when paused")]
    public GameObject[] PausedObjects;

    float oldTimeScale = 1f;

    public UnityAction<bool> OnTogglePause;


    private void Awake()
    {
        Ps = this;
        Paused = false;
        AudioListener.pause = false;
    }

    private void Start()
    {
        playerInput = ActorsManager.AM.GetPlayer().GetComponent<PlayerInputHandler>();
        StartCoroutine(InitialClose());
    }

    private void Update()
    {
        if (GetPause())
            TogglePause(1);
        if (GetTab())
            TogglePause(0);
    }


    public void TogglePause(int menuShown)
    {
        for (int i = 0; i < PausedObjects.Length; i++)
            if (menuShown != i && PausedObjects[i].activeInHierarchy)
                return;

        Paused = !Paused;
        AudioListener.pause = Paused;
        OnTogglePause?.Invoke(Paused);
        playerInput.inputEnabled = !Paused;
        // Pause
        if (Paused)
        {
            Cursor.lockState = CursorLockMode.Confined;
            oldTimeScale = Time.timeScale;
            Time.timeScale = 0;
            PausedObjects[menuShown].SetActive(true);
            foreach (GameObject GO in PlayObjects)
                GO.SetActive(false);
        }

        // Play
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = oldTimeScale;
            PausedObjects[menuShown].SetActive(false);
            if (TrailerMode)
                foreach (GameObject GO in TrailerObjects)
                    GO.SetActive(true);
            else if (!CinematicMode)
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

        Cursor.lockState = CursorLockMode.Locked;
        playerInput.inputEnabled = true;
    }

    public bool GetPause()
    {
        return Input.GetButtonDown("Cancel");
    }

    public bool GetTab()
    {
        return Input.GetButtonDown(PauseB);
    }


    public void BackToMainMenu()
    {
        TogglePause(0);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
