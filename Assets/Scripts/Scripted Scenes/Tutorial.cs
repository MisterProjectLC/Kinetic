using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class DictionaryStringObject : SerializableDictionary<Hermes.PlayerClass, GameObject> { }

public class Tutorial : MonoBehaviour
{
    [Header("Combat Room #1")]
    [SerializeField]
    DictionaryStringObject FirstWeaponModels;
    [SerializeField]
    Pickup FirstWeapon;

    [Header("Combat Room #2")]
    [SerializeField]
    DictionaryStringObject SecondWeaponModels;
    [SerializeField]
    Pickup SecondWeapon;
    [SerializeField]
    GameObject LoadoutExplanation;
    [SerializeField]
    GameTrigger SecondWeaponButton;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Hermes.CurrentClass);

        FirstWeaponModels[Hermes.CurrentClass].SetActive(true);
        FirstWeapon.SubscribeToPickup(OnFirstPickup);

        SecondWeaponModels[Hermes.CurrentClass].SetActive(true);
        SecondWeapon.SubscribeToPickup(OnSecondPickup);

        SecondWeaponButton.SubscribeToTriggerActivate(OnSecondButton);
    }

    void OnFirstPickup()
    {
        
    }

    void OnSecondPickup()
    {
        GameObject player = ActorsManager.AM.GetPlayer().gameObject;
        player.GetComponentInChildren<AmmoManager>().SetAmmo(player.GetComponent<LoadoutManager>().GetWeaponAbilities()[0].WeaponRef, 0);
        LoadoutExplanation.SetActive(true);
        UIOverseer.UIO.GetAmmoIndicator().SetActive(false);
    }

    void OnSecondButton()
    {
        UIOverseer.UIO.GetAmmoIndicator().SetActive(true);
    }
}
