using System.Collections;
using UnityEngine;

public class FrontalShield : SecondaryAbility
{
    [SerializeField]
    GameObject shield;
    Color shieldColor;

    [SerializeField]
    float Duration;

    // Start is called before the first frame update
    void Start()
    {
        shield.GetComponent<Health>().OnDie += OnShieldDeplete;
        shieldColor = shield.GetComponent<MeshRenderer>().material.color;
    }


    public override void Execute()
    {
        StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield()
    {
        shield.SetActive(true);
        shield.GetComponent<Health>().Heal(10000);
        for (int i = 0; i < 5; i++)
        {
            shieldColor.a -= 0.1f;
            shield.GetComponent<MeshRenderer>().material.SetColor("_Color", shieldColor);
            yield return new WaitForSeconds(Duration/5);
        }
        shield.SetActive(false);
    }

    void OnShieldDeplete()
    {
        shield.SetActive(false);
    }
}
