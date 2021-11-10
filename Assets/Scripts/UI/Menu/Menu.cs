using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Menu : MonoBehaviour
{
    [System.Serializable]
    protected struct Submenu
    {
        public Animator animator;
        [HideInInspector]
        public bool enabled;
    }

    [SerializeField]
    protected Animator BackgroundAnimator;

    [SerializeField]
    protected Submenu[] submenus;

    [SerializeField]
    protected Transitions transitions;

    public UnityAction OnTransitionEnded;

    // Start is called before the first frame update
    protected void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        if (BackgroundAnimator)
            BackgroundAnimator.speed = 1f / 60f;
        OnTransitionEnded += VoidTransitionEnded;
    }

    protected void SubmenuButton(int i)
    {
        CloseSubmenus(submenus[i].animator);
        submenus[i].enabled = !submenus[i].enabled;

        if (submenus[i].enabled)
            submenus[i].animator.Play("Open");
        else
            submenus[i].animator.Play("Close");

    }

    void VoidTransitionEnded()
    {
        TransitionEnded();
    }

    protected abstract void TransitionEnded();

    protected void CloseSubmenus(Animator animator)
    {
        for (int i = 0; i < submenus.Length; i++)
            if (submenus[i].animator != animator && submenus[i].enabled)
            {
                submenus[i].enabled = false;
                submenus[i].animator.Play("Close");
            }
    }
}
