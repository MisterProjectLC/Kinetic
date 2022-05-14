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
        GetAnimator();
        Debug.Log("Animator " + animator);
    }

    Animator GetAnimator()
    {
        if (!animator)
        {
            text = GetComponentInChildren<Text>();
            animator = text.GetComponent<Animator>();

        }
        return animator;
    }

    public void Setup(GameObject player)
    {
        AmmoManager ammoManager = player.GetComponentInChildren<AmmoManager>();
        ammoManager.SubscribeToOutOfAmmo(OnOutOfAmmo);
        ammoManager.SubscribeToInfiniteAmmo(OnInfiniteAmmo);
        ammoManager.SubscribeToAmmoUpdate(OnAmmoUpdate);
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
        GetAnimator().SetBool("Animating", inAnimation);
    }


    public void OnAmmoUpdate(Weapon weapon)
    {
        if (infiniteAmmo || currentWeapon != weapon)
            return;

        SetInAnimation(false);
        GetAnimator().SetTrigger("Ping");

        if (!weapon)
            text.text = "";
        else
            text.text = weapon.Ammo.ToString() + "/" + weapon.MaxAmmo.ToString();
    }
}
