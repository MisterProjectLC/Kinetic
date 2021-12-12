using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : Menu
{
    Pause pause;

    [SerializeField]
    protected Transitions transitions;

    new void Start()
    {
        pause = GetComponentInParent<Pause>();
        base.Start();
    }


    public void ResumeButton()
    {
        pause.TogglePause(1);
    }

    public void MenuButton()
    {
        transitions.PlayTransition("ClosingTransition", TransitionEnded);
    }

    protected void TransitionEnded()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
