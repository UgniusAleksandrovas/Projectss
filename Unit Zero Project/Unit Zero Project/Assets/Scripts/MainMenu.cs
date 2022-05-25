using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public float waitTillFreezeTime;
    public GameObject homeMenu;
    public GameObject optionsMenu;
    public GameObject playMenu;
    public GameObject creditsScreen;

    private bool openOptions;
    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    public void Options() {
        anim.SetTrigger("Options");
        openOptions = true;
        DeactivateMenus();
    }

    public void Credits() {
        creditsScreen.SetActive(true);
    }

    public void Play() {
        anim.SetTrigger("Play");
        openOptions = false;
        DeactivateMenus();
    }

    public void Home() {
        anim.SetTrigger("Home");
        openOptions = false;
        DeactivateMenus();
    }

    public void PlayGame() {
        SceneManager.LoadScene("Forest");
    }

    public void Quit() {
        Application.Quit();
    }

    private void DeactivateMenus() {
        homeMenu.SetActive(false);
        optionsMenu.SetActive(false);
        playMenu.SetActive(false);
        creditsScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag( "HomeMenu")) {
            homeMenu.SetActive(true);
        }
        else if (other.CompareTag("OptionsMenu")) {
            if (openOptions == true) {
                optionsMenu.SetActive(true);
            }
        }
        else if (other.CompareTag("PlayMenu")) {
            playMenu.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("HomeMenu")) {
            homeMenu.SetActive(false);
        }
        else if (other.CompareTag("OptionsMenu")) {
            optionsMenu.SetActive(false);
        }
        else if (other.CompareTag("PlayMenu")) {
            playMenu.SetActive(false);
        }
    }
}
