using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoIndicator : MonoBehaviour
{
    bool infiniteAmmo = false;
    bool inAnimation = false;
    Text text;
    Animator animator;

    Weapon currentWeapon = null;

    [SerializeField]
    LocalizedString outOfAmmoText;
    [SerializeField]
    LocalizedString infiniteAmmoText;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        animator = text.GetComponent<Animator>();
    }

    public void Setup(GameObject player)
    {
        player.GetComponentInChildren<AmmoManager>().SubscribeToOutOfAmmo(OnOutOfAmmo);
        player.GetComponentInChildren<AmmoManager>().SubscribeToInfiniteAmmo(OnInfiniteAmmo);
        player.GetComponentInChildren<AmmoManager>().SubscribeToAmmoUpdate(OnAmmoUpdate);
    }

    public void SetCurrentWeapon(Weapon weapon)
    {
        gameObject.SetActive(weapon != null);
        currentWeapon = weapon;
    }

    void OnOutOfAmmo()
    {
        SetInAnimation(true);
        text.text = outOfAmmoText.value;
    }


    void OnInfiniteAmmo(bool isInfinite)
    {
        infiniteAmmo = isInfinite;
        SetInAnimation(isInfinite);
        if (isInfinite)
            text.text = infiniteAmmoText.value;

    }

    void SetInAnimation(bool inAnimation)
    {
        if (this.inAnimation == inAnimation)
            return;
        this.inAnimation = inAnimation;
        animator.SetBool("Animating", inAnimation);
    }


    public void OnAmmoUpdate(Weapon weapon)
    {
        if (infiniteAmmo || currentWeapon != weapon)
            return;

        SetInAnimation(false);
        animator.SetTrigger("Ping");
        Debug.Log("Ping");

        if (!weapon)
            text.text = "";
        else
            text.text = weapon.Ammo.ToString() + "/" + weapon.MaxAmmo.ToString();
    }
}
