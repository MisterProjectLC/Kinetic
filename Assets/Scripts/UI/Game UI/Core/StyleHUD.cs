using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleHUD : MonoBehaviour
{
    [System.Serializable]
    class Line
    {
        public Text text;
        public float total = 0;
        public int category = -1;
        public CanvasGroup canvasGroup;
    }

    [SerializeField]
    List<Line> lines;
    [SerializeField]
    Text AirborneBonus;
    [SerializeField]
    Text TierText;

    [SerializeField]
    FloatBar StyleBar;

    StyleMeter style;
    CanvasGroup styleCanvasGroup;

    [SerializeField]
    float TierMultiplier = 3;

    [SerializeField]
    FloatReference totalStyle;
    int styleTier = 0;
    string[] tiers = {"D", "C", "B", "A", "S", "SS"};
    float fadingClock = 0f;

    [SerializeField]
    float WaitToFade = 5f;

    [SerializeField]
    GameObjectReference PlayerReference;

    // Start is called before the first frame update
    void Start()
    {
        style = PlayerReference.Reference.GetComponentInChildren<StyleMeter>();
        style.OnEvent += OnEvent;
        style.OnBonus += OnBonus;

        totalStyle.Value = 100;

        styleCanvasGroup = GetComponent<CanvasGroup>();
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].canvasGroup = lines[i].text.GetComponent<CanvasGroup>();
        }

        UpdateStyleTotal(0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Fade
        if (fadingClock < WaitToFade)
            fadingClock += Time.deltaTime;
        else
            styleCanvasGroup.alpha -= Time.deltaTime;

        // Decay
        if (totalStyle > 0f)
            UpdateStyleTotal(totalStyle - (styleCanvasGroup.alpha <= 0f ? 4f : 1f) * (styleTier+1) * TierMultiplier * Time.deltaTime);

        // Update lines
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].canvasGroup.alpha > 0f)
            {
                lines[i].canvasGroup.alpha -= 0.15f * Time.deltaTime;
                if (lines[i].canvasGroup.alpha <= 0f)
                {
                    lines[i].text.text = "";
                    lines[i].total = 0;
                    lines[i].category = -1;
                }
            }
        }
    }

    void OnEvent(float amount, int category, string eventText)
    {
        if (amount > 0)
        {
            fadingClock = 0f;
            styleCanvasGroup.alpha = 1f;
        }

        amount *= 10;

        // Find line
        int blankest = 0;
        float lowestAlpha = 1;
        for (int i = 0; i < lines.Count; i++)
        {
            // Get line of the same category
            if (lines[i].category == category)
            {
                blankest = i;
                break;
            }

            // Get unused line
            if (lines[i].canvasGroup.alpha < lowestAlpha)
            {
                lowestAlpha = lines[i].canvasGroup.alpha;
                blankest = i;
            }
        }

        // Setup line
        if (lines[blankest].category != category)
        {
            lines[blankest].total = 0;
            lines[blankest].category = category;
        }
        lines[blankest].total += amount;
        lines[blankest].text.text = eventText + (lines[blankest].total > 0 ? " +" : " ") + (lines[blankest].total + amount).ToString();
        lines[blankest].canvasGroup.alpha = 1f;

        // Setup bar
        amount *= (1 + tiers.Length - styleTier)/2;
        UpdateStyleTotal(totalStyle + amount);
    }

    void OnBonus(int category, bool active)
    {
        switch(category)
        {
            case (int)StyleMeter.Categories.Airborne:
                AirborneBonus.enabled = active;
                break;
        }
    }

    void UpdateStyleTotal(float value)
    {
        float sumStyle = value;

        // Downgrade
        if (sumStyle <= 0f && styleTier > 0)
        {
            styleTier--;
            sumStyle += style.MaxStyle * 10f + 1;
        }

        // Upgrade
        else if (sumStyle >= style.MaxStyle * 10f && styleTier < 5)
        {
            styleTier++;
            sumStyle -= style.MaxStyle * 10f + 1;
        }
        sumStyle = Mathf.Clamp(sumStyle, 0f, style.MaxStyle * 10f);

        // Apply
        TierText.text = tiers[styleTier];

        totalStyle.Value = sumStyle;
        StyleBar.UpdateBar();
    }
}