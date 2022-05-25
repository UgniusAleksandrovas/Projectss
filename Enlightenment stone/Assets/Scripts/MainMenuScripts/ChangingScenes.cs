using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangingScenes : MonoBehaviour
{

    [SerializeField] GameObject DeathScreen;
    [SerializeField] GameObject startMenu;


    public void OnMouseDrag()
    {
        DeathScreen.SetActive(false);
        AudioListener.volume = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnMouseDragRetry()
    {
        DeathScreen.SetActive(false);
        SceneManager.LoadScene("Game");
    }
}
