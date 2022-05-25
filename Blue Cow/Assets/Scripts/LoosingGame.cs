using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoosingGame : MonoBehaviour
{
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject CanvasUI;
    [SerializeField] GameObject AudioManager;
    [SerializeField] GameObject FootSteps;
 

    void Loose()
    {
        loseMenu.SetActive(true);
        CanvasUI.SetActive(false);
        AudioManager.SetActive(false);
        FootSteps.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LostGame")
        {
            Loose();
            Time.timeScale = 0;
        }
    }
}
