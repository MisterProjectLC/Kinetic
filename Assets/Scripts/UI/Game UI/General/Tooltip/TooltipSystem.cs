using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem TS;

    [SerializeField]
    Tooltip tooltip;


    // Start is called before the first frame update
    void Awake()
    {
        TS = this;
    }

    private void Start()
    {
        GetComponent<Pause>().OnTogglePause += OnTogglePause;
    }


    void OnTogglePause(bool paused)
    {
        if (!paused)
            Hide();
    }

    public void Show(string title, string description)
    {
        if (!Pause.Paused)
            return;

        tooltip.Setup(title, description);
        tooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }
}
