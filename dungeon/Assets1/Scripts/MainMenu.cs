using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject GameMenu;
    public GameObject Settings;
    public GameObject Exit;
    public GameObject RuSure;
    public GameObject MenuCanvas;
    public GameObject SettingsMenu;
    public GameObject DropDown;
    public GameObject Slider;
    public GameObject GraphicsDropDown;



    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Debug.Log("Quitted!");
        Application.Quit();
    }
    public void DontQuitGame()
    {
        mainmenu.SetActive(true);
        Settings.SetActive(true);
        Exit.SetActive(true);
        RuSure.SetActive(false);
        GameMenu.SetActive(false);
    }

    public void OpenScenes()
    {
        mainmenu.SetActive(true);
        Settings.SetActive(true);
        Exit.SetActive(false);
        RuSure.SetActive(false);
        GameMenu.SetActive(false);

    }
    public void CloseScenes()
    {
        mainmenu.SetActive(true);
        Settings.SetActive(false);
        Exit.SetActive(false);
        RuSure.SetActive(false);
        GameMenu.SetActive(false);
    }
    public void exitsure()
    {
        mainmenu.SetActive(true);
        Settings.SetActive(true);
        Exit.SetActive(true);
        RuSure.SetActive(true);
        GameMenu.SetActive(false);
        SettingsMenu.SetActive(false);
    }
    public void settings()
    {
        mainmenu.SetActive(false);
        Settings.SetActive(false);
        Exit.SetActive(false);
        RuSure.SetActive(false);
        MenuCanvas.SetActive(false);
        GameMenu.SetActive(false);
    }
    public void GameSettings()
    {
        mainmenu.SetActive(true);
        Settings.SetActive(true);
        Exit.SetActive(true);
        RuSure.SetActive(false);
        GameMenu.SetActive(false);
    }
    public void gamemenu()
    {
        mainmenu.SetActive(true);
        Settings.SetActive(true);
        Exit.SetActive(true);
        RuSure.SetActive(false);
        MenuCanvas.SetActive(true);
        GameMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }
    public void SettingsMenuu()
    {
        RuSure.SetActive(false);
        Exit.SetActive(true);
        MenuCanvas.SetActive(true);
        SettingsMenu.SetActive(true);
        GameMenu.SetActive(false);
    }
    public void Back()
    {
        RuSure.SetActive(false);
        Exit.SetActive(true);
        MenuCanvas.SetActive(true);
        SettingsMenu.SetActive(false);
        Slider.SetActive(false);
        DropDown.SetActive(false);
    }
    public void slider()
    {
        Slider.SetActive(true);
    }
    public void dropdown()
    {
        DropDown.SetActive(true);
    }
    public void graphics()
    {
        GraphicsDropDown.SetActive(true);
    }
}
