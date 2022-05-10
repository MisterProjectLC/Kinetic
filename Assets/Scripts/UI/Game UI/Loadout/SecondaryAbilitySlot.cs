using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryAbilitySlot : AbilitySlot
{
    LocalizedString secondaryAbility;

    private void Start()
    {
        LocalizationSystem.OnLanguageUpdate += UpdateText;
    }

    private void OnDestroy()
    {
        LocalizationSystem.OnLanguageUpdate -= UpdateText;
    }

    public void SetSecondaryAbility(LocalizedString localizedString)
    {
        secondaryAbility = localizedString;
        UpdateText();
    }

    void UpdateText()
    {
        Type = secondaryAbility.value;
        LabelText = secondaryAbility.key;
    }

    public override string GetDropType()
    {
        return secondaryAbility.value;
    }
}
