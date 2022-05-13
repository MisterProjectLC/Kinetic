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

    static Color weaponColor = new Color(104f/255, 24f / 255, 36f / 255);
    static Color abilityColor = new Color(41f / 255, 25f / 255, 69f / 255);

    public RectTransform CenterTransform { get; set; }


    private void Awake()
    {
        AbilitySize = CooldownRect.sizeDelta;
        gameObject.SetActive(false);
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

    }


    public void LoadoutUpdate(Ability currentAbility)
    {
        Image image = CooldownRect.GetComponent<Image>();

        if (AdditionalAbility)
            Destroy(AdditionalAbility);
        if (CenterAdditionalAbility)
            Destroy(CenterAdditionalAbility);

        if (!currentAbility)
        {
            gameObject.SetActive(false);
            return;
        }

        if (currentAbility.abilityView)
            AdditionalAbility = CreateInstanceView(currentAbility, currentAbility.abilityView, transform);
        if (currentAbility.centerAbilityView)
            CenterAdditionalAbility = CreateInstanceView(currentAbility, currentAbility.centerAbilityView, CenterTransform);

        gameObject.SetActive(true);
        Label.text = currentAbility.LocalizedName.value;

        if (currentAbility is WeaponAbility)
            image.color = weaponColor;
        else
            image.color = abilityColor;
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
