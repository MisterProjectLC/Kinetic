using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityView : MonoBehaviour
{
    [SerializeField]
    RectTransform CooldownRect;
    [SerializeField]
    Text Label;

    Vector2 AbilitySize;
    Vector2 AdditionalAbilityPosition = new Vector2(-260, 24);

    GameObject AdditionalAbility;
    GameObject CenterAdditionalAbility;

    public RectTransform CenterTransform { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        AbilitySize = CooldownRect.sizeDelta;
    }


    public void AbilityUpdate(Ability currentAbility)
    {
        if (!currentAbility)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        CooldownRect.sizeDelta = AbilitySize * new Vector2(Mathf.Clamp01(1f - (currentAbility.Timer / currentAbility.Cooldown)), 1f);
    }


    public void DeviceUpdate(Device device)
    {
        Image image = CooldownRect.GetComponent<Image>();
        Color color = image.color;
        if (Label.text == device.gameObject.name)
            image.color = new Color(color.r, color.g, color.b, 1);
        else
            image.color = new Color(color.r, color.g, color.b, 0.4f);
    }


    public void LoadoutUpdate(Ability currentAbility)
    {
        Image image = CooldownRect.GetComponent<Image>();

        if (AdditionalAbility)
            Destroy(AdditionalAbility);
        if (CenterAdditionalAbility)
            Destroy(CenterAdditionalAbility);

        if (currentAbility == null)
        {
            image.gameObject.SetActive(false);
            return;
        }

        if (currentAbility.abilityView)
            AdditionalAbility = CreateInstanceView(currentAbility, currentAbility.abilityView, transform);
        if (currentAbility.centerAbilityView)
            CenterAdditionalAbility = CreateInstanceView(currentAbility, currentAbility.centerAbilityView, CenterTransform);

        image.gameObject.SetActive(true);
        Label.text = currentAbility.LocalizedName.value;
        CooldownRect.sizeDelta = Vector2.zero;
    }

    GameObject CreateInstanceView(Ability ability, GameObject view, Transform instanceParent)
    {
        try
        {
            GameObject instance = Instantiate(view);
            IAbilityAdditionalView additionalView = instance.GetComponent<IAbilityAdditionalView>();
            additionalView.Setup(ability);
            additionalView.transform.parent = instanceParent;
            additionalView.GetComponent<RectTransform>().anchoredPosition = AdditionalAbilityPosition;
            return instance;
        }
        catch
        {
            Debug.LogError("This Ability's View doesn't have an AdditionalView component!");
            return null;
        }
    }
}
