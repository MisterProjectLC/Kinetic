using UnityEngine;
using UnityEngine.UI;

public class StyleHUD : MonoBehaviour
{
    StyleMeter style;
    Text text;
    CanvasGroup canvasGroup;

    int lines = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();

        style = ActorsManager.AM.GetPlayer().GetComponentInChildren<StyleMeter>();
        style.OnEvent += OnEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha > 0f) { 
            canvasGroup.alpha -= 0.15f*Time.deltaTime;
            if (canvasGroup.alpha <= 0f)
            {
                text.text = "";
                lines = 0;
            }
        } 
    }

    void OnEvent(float amount, string eventText)
    {
        amount *= 10;

        text.text = eventText + (amount > 0 ? " +" : " ") + amount.ToString() + "\n" + text.text;
        if (lines >= 4)
            text.text = string.Join("\n", text.text.Split('\n'), 0, 4);
        else
            lines++;
        canvasGroup.alpha = 1f;
    }
}