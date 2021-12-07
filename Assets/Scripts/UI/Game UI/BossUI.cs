using UnityEngine;
using UnityEngine.UI;

public class BossUI : CustomUI
{
    [Range(0f, 1f)]
    public float Percentage = 1f;
    float oldPercentage = 1f;

    [SerializeField]
    Text TitleText;
    [SerializeField]
    Text SubtitleText;

    void OnEnable()
    {
        TitleText.material.SetFloat("_PercentageVisible", 1f);
        SubtitleText.material.SetFloat("_PercentageVisible", 1f);
        oldPercentage = Percentage;
        GetComponent<Animator>().Play("BossIntroduction");
    }

    private void Update()
    {
        if (oldPercentage != Percentage)
        {
            TitleText.material.SetFloat("_PercentageVisible", Percentage);
            SubtitleText.material.SetFloat("_PercentageVisible", Percentage);
            //TitleText.SetMaterialDirty();
            //SubtitleText.SetMaterialDirty();
            oldPercentage = Percentage;
        }
    }
}
