using UnityEngine;
using UnityEngine.UI;

public class AlterWorldUI : MonoBehaviour
{
    [SerializeField]
    GameTrigger gameTrigger;

    [SerializeField]
    LocalizedString newText;

    // Start is called before the first frame update
    void Awake()
    {
        gameTrigger.OnTriggerActivate += AlterUI;
        gameTrigger.OnTriggerDestroy += AlterUI;

        if (GetComponent<UniqueID>())
            GetComponent<UniqueID>().OnObjectRegistered += AlterUI;
    }

    void AlterUI()
    {
        GetComponentInChildren<Text>().text = newText.value;
    }
}
