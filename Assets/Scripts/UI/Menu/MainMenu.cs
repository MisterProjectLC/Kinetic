using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    bool quitting = false;

    [SerializeField]
    protected Animator BackgroundAnimator;

    [SerializeField]
    protected Transitions transitions;

    new void Start()
    {
        Time.timeScale = 1f;
        if (BackgroundAnimator)
            BackgroundAnimator.speed = 1f / 60f;
        base.Start();
    }


    public void CharacterButton()
    {
        quitting = false;
        Hermes.SpawnPosition = Vector3.zero;
        Hermes.SpawnAreas = new List<string>() { "L_Engi_Tutorial" };
        Hermes.SpawnAbilities.Clear();
        transitions.PlayTransition("ClosingTransition", TransitionEnded);
    }

    public void QuitButton()
    {
        quitting = true;
        transitions.PlayTransition("ClosingTransition", TransitionEnded);
    }

    void TransitionEnded()
    {
        if (quitting)
            Application.Quit();
        else
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
