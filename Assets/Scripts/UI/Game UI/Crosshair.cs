using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("Crosshair")]
    [SerializeField]
    Image crosshair;
    [SerializeField]
    List<Sprite> crosshairs;
    [SerializeField]
    Sprite expandedCrosshair;

    [SerializeField]
    float crosshairAnimationTime = 0.4f;
    [SerializeField]
    int crosshairExpandedFrame = 3;
    float crosshairClock = 0f;
    int crosshairFrame = 0;

    [Header("Hitmarker")]
    [SerializeField]
    Image hitmarker;
    [SerializeField]
    List<Sprite> hitmarkers;

    [SerializeField]
    float hitmarkerAnimationTime = 0.1f;
    float hitmarkerClock = 0f;
    int hitmarkerFrame = 0;

    AudioSource audioSource;

    LoadoutManager loadoutManager;
    int savedLoadout = 0;

    Animator animator;

    private void Start()
    {
        foreach (Weapon weapon in ActorsManager.AM.GetPlayer().GetComponentsInChildren<Weapon>())
            weapon.OnFire += ExpandCrosshair;

        foreach (Attack attack in ActorsManager.AM.GetPlayer().GetComponentsInChildren<Attack>())
        {
            Debug.Log(attack.gameObject.name);
            attack.OnAttack += ActivateHitmarker;
        }

        audioSource = GetComponent<AudioSource>();

        loadoutManager = ActorsManager.AM.GetPlayer().GetComponent<LoadoutManager>();
        loadoutManager.OnLoadoutSwitch += RotateCrosshair;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        crosshairClock += Time.deltaTime;
        if (crosshairClock > crosshairAnimationTime)
        {
            crosshairClock = 0f;
            crosshairFrame = (crosshairFrame + 1) % crosshairs.Count;
            crosshair.sprite = crosshairs[crosshairFrame];
        }

        if (!hitmarker.enabled)
            return;

        hitmarkerClock += Time.deltaTime;
        if (hitmarkerClock > hitmarkerAnimationTime)
        {
            hitmarkerClock = 0f;
            hitmarkerFrame++;
            if (hitmarkerFrame < hitmarkers.Count)
            {
                hitmarker.color = new Color(hitmarker.color.r, hitmarker.color.g, hitmarker.color.b, hitmarker.color.a - 0.15f);
                hitmarker.sprite = hitmarkers[hitmarkerFrame];
            }
            else
            {
                hitmarker.color = new Color(hitmarker.color.r, hitmarker.color.g, hitmarker.color.b, 0f);
                hitmarker.enabled = false;
            }
        }
    }


    void ExpandCrosshair()
    {
        crosshairFrame = crosshairExpandedFrame;
        crosshair.sprite = expandedCrosshair;
        crosshairClock = -0.1f;
    }


    void ActivateHitmarker(GameObject target, float multiplier, int damage)
    {
        if (damage <= 0)
            return;

        hitmarker.enabled = true;
        hitmarkerFrame = 0;
        hitmarker.color = new Color(hitmarker.color.r, hitmarker.color.g, hitmarker.color.b, Mathf.Clamp(hitmarker.color.a + damage * 0.3f, 0f, 1f));
        hitmarker.sprite = hitmarkers[0];

        audioSource.volume = 0.5f + hitmarker.color.a/2;
        audioSource.Play();
    }

    void RotateCrosshair()
    {
        if (savedLoadout > loadoutManager.currentLoadout)
            animator.Play("CrosshairRotateLeft");
        else if (savedLoadout < loadoutManager.currentLoadout)
            animator.Play("CrosshairRotateRight");

        savedLoadout = loadoutManager.currentLoadout;
    }
}
