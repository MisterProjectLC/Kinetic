using UnityEngine;
using UnityEngine.UI;

public class Localizer : MonoBehaviour
{
    [SerializeField]
    LocalizedString Key;

    // Start is called before the first frame update
    void Awake()
    {
        LocalizationSystem.OnLanguageUpdate += UpdateText;
        UpdateText();
    }


    public string GetKey()
    {
        return Key.key;
    }

    public void UpdateKey(string key)
    {
        Key.key = key;
        UpdateText();
    }


    void UpdateText()
    {
        if (Key.key == "")
            return;

        if (GetComponentInChildren<Text>())
            GetComponentInChildren<Text>().text = Key.value;
    }
}
