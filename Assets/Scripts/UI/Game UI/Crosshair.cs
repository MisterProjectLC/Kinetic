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
    Color hitmarkerColor = Color.white;

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
            attack.OnAttack += ActivateHitmarker;

        audioSource = GetComponent<AudioSource>();

        loadoutManager = ActorsManager.AM.GetPlayer().GetComponent<LoadoutManager>();
        loadoutManager.OnLoadoutSwitch += RotateCrosshair;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Crosshair animation
        crosshairClock += Time.deltaTime;
        if (crosshairClock > crosshairAnimationTime)
        {
            crosshairClock = 0f;
            crosshairFrame = (crosshairFrame + 1) % crosshairs.Count;
            crosshair.sprite = crosshairs[crosshairFrame];
        }

        // Hitmarker animation
        if (!hitmarker.enabled)
            return;

        hitmarkerClock += Time.deltaTime;
        if (hitmarkerClock > hitmarkerAnimationTime)
        {
            hitmarkerClock = 0f;

            if (hitmarkerColor.g < 1f)
            {
                hitmarkerColor.g += 0.25f;
                hitmarkerColor.b += 0.25f;
            }

            hitmarkerFrame++;
            if (hitmarkerFrame < hitmarkers.Count)
            {
                hitmarkerColor.a -= 0.15f;
                hitmarker.color = hitmarkerColor;
                hitmarker.sprite = hitmarkers[hitmarkerFrame];
            }
            else
            {
                hitmarkerColor.a = 0f;
                hitmarker.color = hitmarkerColor;
                hitmarker.enabled = false;
            }
        }
    }


    void ActivateHitmarker(GameObject target, float multiplier, int damage)
    {
        if (damage <= 0)
            return;

        hitmarker.enabled = true;
        hitmarkerFrame = 0;
        hitmarker.sprite = hitmarkers[0];

        if (multiplier > 1)
        {
            hitmarkerColor.g = 0f;
            hitmarkerColor.b = 0f;
        }
        hitmarkerColor.a = Mathf.Clamp(hitmarkerColor.a + damage * 0.3f, 0f, 1f);
        hitmarker.color = hitmarkerColor;

        audioSource.volume = 0.5f + hitmarker.color.a/2;
        audioSource.Play();
    }


    void ExpandCrosshair()
    {
        crosshairFrame = crosshairExpandedFrame;
        crosshair.sprite = expandedCrosshair;
        crosshairClock = -0.1f;
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
