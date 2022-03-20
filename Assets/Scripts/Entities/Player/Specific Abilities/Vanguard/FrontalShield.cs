using System.Collections;
using UnityEngine;

public class FrontalShield : SecondaryAbility
{
    [SerializeField]
    GameObject shield;
    Color shieldColor;
    float originalAlpha = 1f;

    [SerializeField]
    float Duration;

    [SerializeField]
    AudioClip breakSound;

    // Start is called before the first frame update
    void Start()
    {
        shield.GetComponent<Health>().OnDie += OnShieldDeplete;
        shieldColor = shield.GetComponent<MeshRenderer>().material.color;
        originalAlpha = shieldColor.a;
    }


    public override void Execute(Input input)
    {
        StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield()
    {
        shield.SetActive(true);
        shield.GetComponent<Health>().Heal(10000);
        shieldColor.a = originalAlpha;
        for (int i = 0; i < 5; i++)
        {
            shieldColor.a -= originalAlpha/5;
            shield.GetComponent<MeshRenderer>().material.SetColor("_Color", shieldColor);
            yield return new WaitForSeconds(Duration/5);
        }
        shield.SetActive(false);
    }

    void OnShieldDeplete()
    {
        PlaySound(breakSound);
        shield.SetActive(false);
    }
}
