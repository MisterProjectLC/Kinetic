using System.Collections;
using UnityEngine;

public class TutorialUI : CustomUI
{
    [SerializeField]
    GameObjectReference player;

    // Start is called before the first frame update
    protected new void Start()
    {
        StartCoroutine(DisableStyleDrain());
        base.Start();
    }

    IEnumerator DisableStyleDrain()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        player.Reference.GetComponent<StyleMeter>().DrainActive = false;
    }

    void OnEnable()
    {
        GetComponent<Animator>().Play("TutorialControls");
    }
}
