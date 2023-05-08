using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject settingMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        settingMenu.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void SettingMenuOpen()
    {
        mainMenu.SetActive(false);
        settingMenu.SetActive(true);
    }

    public void backToMainMenu()
    {
        mainMenu.SetActive(true);
        settingMenu.SetActive(false);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
