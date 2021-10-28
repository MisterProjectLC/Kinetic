using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [System.Serializable]
    struct Submenu
    {
        public Animator animator;
        [HideInInspector]
        public bool enabled;
    }

    [SerializeField]
    Animator BackgroundAnimator;

    [SerializeField]
    Submenu[] submenus;

    [SerializeField]
    Transitions transitions;

    bool quitting = false;

    public UnityAction OnTransitionEnded;

    // Start is called before the first frame update
    void Start()
    {
	Cursor.lockState = CursorLockMode.Confined;
        BackgroundAnimator.speed = 1f/60f;
        OnTransitionEnded += TransitionEnded;
    }

    public void PlayButton()
    {
        SubmenuButton(0);
    }

    public void CharacterButton()
    {
        quitting = false;
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


    void SubmenuButton(int i)
    {
        CloseSubmenus(submenus[i].animator);
        submenus[i].enabled = !submenus[i].enabled;

        if (submenus[i].enabled)
            submenus[i].animator.Play("Open");
        else
            submenus[i].animator.Play("Close");

    }


    void TransitionEnded()
    {
        if (quitting)
            Application.Quit();
        else
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    void CloseSubmenus(Animator animator)
    {
        for (int i = 0; i < submenus.Length; i++)
            if (submenus[i].animator != animator && submenus[i].enabled)
            {
                submenus[i].enabled = false;
                submenus[i].animator.Play("Close");
            }
    }
}
