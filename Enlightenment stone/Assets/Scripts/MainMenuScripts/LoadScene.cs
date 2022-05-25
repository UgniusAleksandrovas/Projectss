using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadScene : MonoBehaviour
{

    [SerializeField] GameObject BackGroundButton;
    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject OptionsButton;
    [SerializeField] GameObject QuitButton;
    [SerializeField] GameObject GraphicsButton;
    [SerializeField] GameObject AudioButton;
    [SerializeField] GameObject BackButton;
    [SerializeField] GameObject NoButton;
    [SerializeField] GameObject QuitConfirmation;
    [SerializeField] GameObject DropDownGraphics;
    [SerializeField] GameObject ResolutionDropDown;
    [SerializeField] GameObject FullScreen;
    [SerializeField] GameObject Slider;

    public void Awake()
    {
        PlayButton.SetActive(true);
        OptionsButton.SetActive(true);
        QuitButton.SetActive(true);
    }

    public void OnMouseDrag()
    {
        StartCoroutine(changingScene());
    }

    public IEnumerator changingScene()
    {
        PlayButton.SetActive(false);
        OptionsButton.SetActive(false);
        QuitButton.SetActive(false);
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("Game");
    }

    public void onMouseDragOptions()
    {
        StartCoroutine(OptionsScreen());
    }

    public IEnumerator OptionsScreen()
    {
        yield return new WaitForSeconds(1.8f);
        BackGroundButton.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        GraphicsButton.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        AudioButton.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        BackButton.SetActive(true);
    }

    public void onMouseDragBack()
    {
        StartCoroutine(BackScreen());
    }

    public IEnumerator BackScreen()
    {
        BackGroundButton.SetActive(false);
        yield return new WaitForSeconds(1.8f);
        PlayButton.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        OptionsButton.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        QuitButton.SetActive(true);
        GraphicsButton.SetActive(false);
        AudioButton.SetActive(false);
        BackButton.SetActive(false);
        DropDownGraphics.SetActive(false);
        ResolutionDropDown.SetActive(false);
        FullScreen.SetActive(false);
        Slider.SetActive(false);
    }

    public void onMouseDragNoButton()
    {
        StartCoroutine(No());
    }

    public IEnumerator No()
    {
        QuitConfirmation.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        PlayButton.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        OptionsButton.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        QuitButton.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("You Quit");
    }

    public void Graphcis()
    {
        DropDownGraphics.SetActive(true);
        ResolutionDropDown.SetActive(true);
        FullScreen.SetActive(true);
        Slider.SetActive(false);
    }

    public void Audio()
    {
        DropDownGraphics.SetActive(false);
        ResolutionDropDown.SetActive(false);
        FullScreen.SetActive(false);
        Slider.SetActive(true);
    }
}
