using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayAction : MonoBehaviour
{
    [SerializeField] PlayerHop playerAnimationScript;
    bool canUseButtons;

    ButtonType selectedButton = ButtonType.singleplayer;

    [SerializeField] MenuButton singleplayer;
    [SerializeField] MenuButton multiplayer;
    [SerializeField] MenuButton settings;
    [SerializeField] MenuButton quit;

    [Space(20)]
    [SerializeField] float buttonStartDelay = 1f;
    [SerializeField] float buttonPressDelay = 2f;

    private void Start()
    {
        StartCoroutine(WaitToActivateButtons());
    }

    void Update()
    {
        if (canUseButtons)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                playerAnimationScript.StartJump();
                if (selectedButton == ButtonType.singleplayer)
                {
                    Singleplayer();
                }
                else if (selectedButton == ButtonType.multiplayer)
                {
                    Multiplayer();
                }
                else if (selectedButton == ButtonType.settings)
                {
                    //Settings();
                }
                else if (selectedButton == ButtonType.quit)
                {
                    QuitTheGame();
                }
                SetButtonsUsable(false);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedButton > ButtonType.singleplayer)
                {
                    selectedButton--;
                }
                else
                {
                    selectedButton = ButtonType.quit;
                }
                EnableButton(selectedButton);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedButton < ButtonType.quit)
                {
                    selectedButton++;
                }
                else
                {
                    selectedButton = ButtonType.singleplayer;
                }
                EnableButton(selectedButton);
            }
        }
    }

    public void Singleplayer()
    {
        singleplayer.text.gameObject.SetActive(false);
        StartCoroutine(SingleplayerDelay());
    }
    IEnumerator SingleplayerDelay()
    {
        yield return new WaitForSeconds(buttonPressDelay);
        SceneManager.LoadScene("Cutscene (Test)");
    }

    public void Multiplayer()
    {
        multiplayer.text.gameObject.SetActive(false);
        StartCoroutine(MultiplayerDelay());
    }
    IEnumerator MultiplayerDelay()
    {
        yield return new WaitForSeconds(buttonPressDelay);
        SceneManager.LoadScene("LobbyScene");
    }

    public void Settings()
    {
        settings.text.gameObject.SetActive(false);
        StartCoroutine(SettingsDelay());
    }
    IEnumerator SettingsDelay()
    {
        yield return new WaitForSeconds(buttonPressDelay);
        // Load new scene here or open some UI
    }

    public void QuitTheGame()
    {
        quit.text.gameObject.SetActive(false);
        StartCoroutine(QuittingGame());
    }
    IEnumerator QuittingGame()
    {
        yield return new WaitForSeconds(buttonPressDelay);
        Application.Quit();
    }

    public void SelectButton(ButtonType buttonType)
    {
        selectedButton = buttonType;
        EnableButton(selectedButton);
    }

    void EnableButton(ButtonType buttonType)
    {
        DisableAllButtons();
        if (buttonType == ButtonType.singleplayer)
        {
            singleplayer.image.color = singleplayer.enabledColor;
            singleplayer.text.color = singleplayer.enabledColor;
            singleplayer.light.intensity = singleplayer.enabledIntensity;
            singleplayer.volumetricLightShaft.SetActive(true);
            singleplayer.sfx.Play();
            playerAnimationScript.SetTargetPosition(singleplayer.button.transform.position);
        }
        else if (buttonType == ButtonType.multiplayer)
        {
            multiplayer.image.color = multiplayer.enabledColor;
            multiplayer.text.color = multiplayer.enabledColor;
            multiplayer.light.intensity = multiplayer.enabledIntensity;
            multiplayer.volumetricLightShaft.SetActive(true);
            multiplayer.sfx.Play();
            playerAnimationScript.SetTargetPosition(multiplayer.button.transform.position);
        }
        else if (buttonType == ButtonType.settings)
        {
            settings.image.color = settings.enabledColor;
            settings.text.color = settings.enabledColor;
            settings.light.intensity = settings.enabledIntensity;
            settings.volumetricLightShaft.SetActive(true);
            settings.sfx.Play();
            playerAnimationScript.SetTargetPosition(settings.button.transform.position);
        }
        else if (buttonType == ButtonType.quit)
        {
            quit.image.color = quit.enabledColor;
            quit.text.color = quit.enabledColor;
            quit.light.intensity = quit.enabledIntensity;
            quit.volumetricLightShaft.SetActive(true);
            quit.sfx.Play();
            playerAnimationScript.SetTargetPosition(quit.button.transform.position);
        }
    }

    public void DisableAllButtons()
    {
        singleplayer.image.color = singleplayer.disabledColor;
        singleplayer.text.color = singleplayer.disabledColor;
        singleplayer.light.intensity = singleplayer.disabledIntensity;
        singleplayer.volumetricLightShaft.SetActive(false);

        multiplayer.image.color = multiplayer.disabledColor;
        multiplayer.text.color = multiplayer.disabledColor;
        multiplayer.light.intensity = multiplayer.disabledIntensity;
        multiplayer.volumetricLightShaft.SetActive(false);

        settings.image.color = settings.disabledColor;
        settings.text.color = settings.disabledColor;
        settings.light.intensity = settings.disabledIntensity;
        settings.volumetricLightShaft.SetActive(false);

        quit.image.color = quit.disabledColor;
        quit.text.color = quit.disabledColor;
        quit.light.intensity = quit.disabledIntensity;
        quit.volumetricLightShaft.SetActive(false);
    }

    IEnumerator WaitToActivateButtons()
    {
        canUseButtons = false;
        DisableAllButtons();
        yield return new WaitForSeconds(buttonStartDelay);
        SetButtonsUsable(true);
        EnableButton(ButtonType.singleplayer);
    }

    void SetButtonsUsable(bool canUse)
    {
        canUseButtons = canUse;
        MenuButtons[] buttons = FindObjectsOfType<MenuButtons>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].canUse = canUse;
        }
    }
}

public enum ButtonType
{
    singleplayer,
    multiplayer,
    settings,
    quit
}

[System.Serializable]
public struct MenuButton
{
    public GameObject button;

    public Text text;
    public Image image;
    public Color enabledColor;
    public Color disabledColor;

    public Light light;
    public float enabledIntensity;
    public float disabledIntensity;
    public GameObject volumetricLightShaft;

    public AudioSource sfx;
}
