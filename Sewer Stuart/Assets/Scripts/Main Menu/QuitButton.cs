using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MainMenuButton, IMenuButton
{
    [Header("Quit Settings")]
    [SerializeField] float quitDelay = 2f;

    public void OnButtonUse()
    {
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(quitDelay);
        Application.Quit();
    }

    public void QuitGameNow()
    {
        Application.Quit();
    }
}
