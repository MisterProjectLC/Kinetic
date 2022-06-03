using UnityEngine;

public class Menu : MonoBehaviour
{
    [System.Serializable]
    protected struct Submenu
    {
        public Animator animator;
        [HideInInspector]
        public bool enabled;
    }

    [SerializeField]
    protected Submenu[] submenus;

    [SerializeField]
    int StartSubmenu = -1;


    // Start is called before the first frame update
    protected void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        for (int i = 0; i < submenus.Length; i++)
            submenus[i].enabled = false;
    }

    private void OnEnable()
    {
        if (StartSubmenu != -1 && !submenus[StartSubmenu].enabled)
            SubmenuButton(StartSubmenu);
    }

    public virtual void SubmenuButton(int i)
    {
        CloseSubmenus(submenus[i].animator);
        submenus[i].enabled = !submenus[i].enabled;

        if (submenus[i].enabled)
            submenus[i].animator.Play("Open");
        else
            submenus[i].animator.Play("Close");

    }

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
