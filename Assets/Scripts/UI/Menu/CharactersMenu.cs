using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharactersMenu : MonoBehaviour
{
    MainMenu mainMenu;

    void Start()
    {
        mainMenu = GetComponentInParent<MainMenu>();
    }

    public void NinjaButton()
    {
        Hermes.CurrentClass = Hermes.PlayerClass.Ninja;
        mainMenu.CharacterButton();
    }

    public void VanguardButton()
    {
        Hermes.CurrentClass = Hermes.PlayerClass.Vanguard;
        mainMenu.CharacterButton();
    }
}
