using UnityEngine;
using UnityEngine.UI;

public class AlterWorldUI : MonoBehaviour
{
    [SerializeField]
    GameTrigger gameTrigger;

    [SerializeField]
    string newText;

    // Start is called before the first frame update
    void Start()
    {
        gameTrigger.OnTriggerActivate += AlterUI;
    }

    void AlterUI()
    {
        GetComponentInChildren<Text>().text = newText;
    }
}
