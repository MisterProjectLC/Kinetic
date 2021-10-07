using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator BackgroundAnimator;
    public Animator GeneralAnimator;
    public Transitions transitions;

    UnityAction OnTransitionEnded;
    bool quitting = false;
    bool optionsEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        BackgroundAnimator.speed = 1f/60f;
        OnTransitionEnded += TransitionEnded;
    }

    public void PlayButton()
    {
        quitting = false;
        transitions.PlayTransition("ClosingTransition", OnTransitionEnded);
    }

    public void OptionsButton()
    {
        optionsEnabled = !optionsEnabled;

        if (optionsEnabled)
            GeneralAnimator.Play("OptionsOpen");
        else
            GeneralAnimator.Play("OptionsClose");
    }

    public void QuitButton()
    {
        quitting = true;
        transitions.PlayTransition("ClosingTransition", OnTransitionEnded);
    }

    void TransitionEnded()
    {
        if (quitting)
            Application.Quit();
        else
            SceneManager.LoadScene("Arena", LoadSceneMode.Single);
    }
}
