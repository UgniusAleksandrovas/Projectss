using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject pauseButtons;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] AudioSource pauseAudioSource;
    bool paused;
    bool gameOver;
    TimeManager timeManager;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    void OnEnable()
    {
        EventManager.OnGameOver += GameOver;
    }

    void OnDisable()
    {
        EventManager.OnGameOver -= GameOver;
    }

    private void GameOver()
    {
        gameOver = true;
    }

    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (paused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
                pauseAudioSource.Play();
            }
        }
    }

    public void PauseGame()
    {
        paused = true;
        timeManager.ChangeTimeScale(0f);
        pauseScreen.SetActive(true);
        pauseButtons.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        paused = false;
        timeManager.ChangeTimeScale(1f);
        pauseScreen.SetActive(false);
    }

    public void RestartLevel()
    {
        timeManager.ChangeTimeScale(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        timeManager.ChangeTimeScale(1f);
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
