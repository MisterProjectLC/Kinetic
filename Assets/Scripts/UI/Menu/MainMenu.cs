using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    bool quitting = false;


    new void Start()
    {
        Time.timeScale = 1f;
        base.Start();
    }

    public void PlayButton()
    {
        SubmenuButton(0);
    }

    public void CharacterButton()
    {
        quitting = false;
        Hermes.SpawnPosition = Vector3.zero;
        Hermes.SpawnAreas = new List<string>() { "L_Engi_Tutorial" };
        transitions.PlayTransition("ClosingTransition", OnTransitionEnded);
    }

    public void OptionsButton()
    {
        SubmenuButton(1);
    }

    public void QuitButton()
    {
        quitting = true;
        transitions.PlayTransition("ClosingTransition", OnTransitionEnded);
    }

    protected override void TransitionEnded()
    {
        if (quitting)
            Application.Quit();
        else
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
