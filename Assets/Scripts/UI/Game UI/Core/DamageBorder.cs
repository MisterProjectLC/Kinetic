using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBorder : MonoBehaviour
{
    CanvasGroup canvasGroup;
    AudioSource audioSource;

    [SerializeField]
    float alphaIncrease = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        ActorsManager.AM.GetPlayer().GetComponent<Health>().OnDamage += OnDamage;
        ActorsManager.AM.GetPlayer().GetComponent<Health>().OnHeal += OnHeal;
    }

    private void Update()
    {
        if (canvasGroup.alpha > 0)
            canvasGroup.alpha -= canvasGroup.alpha * 0.4f * Time.deltaTime;
    }


    void OnDamage(int damage)
    {
        audioSource.Play();
        canvasGroup.alpha = alphaIncrease * damage;
    }


    void OnHeal(int heal)
    {
        canvasGroup.alpha = 0f;
    }

}
