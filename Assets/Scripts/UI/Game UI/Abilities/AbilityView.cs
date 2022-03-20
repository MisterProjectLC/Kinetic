using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityView : MonoBehaviour
{
    [SerializeField]
    RectTransform CooldownRect;
    [SerializeField]
    Text Label;
    [SerializeField]
    Text AmmoLabel;

    Vector2 AbilitySize;

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

        bool isWeapon = currentAbility is WeaponAbility;
        if (isWeapon)
        {
            Weapon weapon = ((WeaponAbility)currentAbility).WeaponRef;
            string text = weapon.Ammo.ToString() + "/" + weapon.MaxAmmo.ToString();
            AmmoLabel.text = text;
        }

        AmmoLabel.transform.parent.gameObject.SetActive(isWeapon);
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

        if (currentAbility == null)
        {
            image.gameObject.SetActive(false);
            return;
        }

        image.gameObject.SetActive(true);
        Label.text = currentAbility.DisplayName;
        CooldownRect.sizeDelta = Vector2.zero;
    }
}
