using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    Pause pause;

    new void Start()
    {
        pause = GetComponentInParent<Pause>();
        base.Start();
    }


    public void ResumeButton()
    {
        pause.TogglePause(1);
    }

    public void OptionsButton()
    {
        SubmenuButton(0);
    }

    public void MenuButton()
    {
        transitions.PlayTransition("ClosingTransition", OnTransitionEnded);
    }

    protected override void TransitionEnded()
    {
        Debug.Log("AAAA");
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
