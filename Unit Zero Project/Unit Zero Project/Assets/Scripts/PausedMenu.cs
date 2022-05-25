using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenu : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject pauseUI;
    public GameObject optionsUI;

    public bool paused;
    private FPSController thePlayer;

    private void Awake() {
        Time.timeScale = 1;
        thePlayer = FindObjectOfType<FPSController>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            pauseMenu.SetActive(paused);
            if (paused) {
                Pause();
            }
            else {
                Continue();
            }
        }
    }

    public void Pause() {
        thePlayer.GetComponent<Inventory>().showInventory = false;
        thePlayer.GetComponent<Inventory>().UpdateInventory();
        paused = true;
        thePlayer.freezeRotation = true;
        thePlayer.freezePosition = true;
        thePlayer.lockMouse = false;
        pauseMenu.SetActive(true);
        pauseUI.SetActive(true);
        optionsUI.SetActive(false);
        Time.timeScale = 0;
    }

    public void Continue() {
        paused = false;
        thePlayer.freezeRotation = false;
        thePlayer.freezePosition = false;
        thePlayer.lockMouse = true;
        pauseMenu.SetActive(false);
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenOptions() {
        pauseUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void Back() {
        pauseUI.SetActive(true);
        optionsUI.SetActive(false);
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadBossFight() {
        SceneManager.LoadScene("BossFight");
    }

    public void MainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit() {
        Application.Quit();
    }
}
