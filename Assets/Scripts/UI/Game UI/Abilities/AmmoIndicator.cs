using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoIndicator : MonoBehaviour
{
    bool inAnimation = false;
    Text text;
    Animator animator;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        animator = text.GetComponent<Animator>();
    }

    public void Setup(GameObject player)
    {
        player.GetComponentInChildren<AmmoManager>().SubscribeToInfiniteAmmo(OnInfiniteAmmo);
    }

    public void OnOutOfAmmo()
    {
        SetInAnimation(true);
        text.text = "OUT OF AMMO";
    }


    public void OnInfiniteAmmo(bool isInfinite)
    {
        SetInAnimation(isInfinite);
        if (isInfinite)
            text.text = "NO AMMO LIMIT";

    }

    void SetInAnimation(bool inAnimation)
    {
        this.inAnimation = inAnimation;
        animator.SetBool("Animating", inAnimation);
    }


    public void UpdateWeapon(Weapon weapon)
    {
        if (inAnimation)
            return;

        animator.SetTrigger("Ping");
        Debug.Log("Ping");

        if (!weapon)
            text.text = "";
        else
            text.text = weapon.Ammo.ToString() + "/" + weapon.MaxAmmo.ToString();
    }
}
