using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health {

    public Image bloodOverlay;
    public Image gameOverOverlay;
    public GameObject gameOverUI;
    public GameObject backUpCam;
    
    public override void UpdateHealth() {
        bloodOverlay.color = new Color(180, 0, 0, 1 - ((float)health / (float)maxHealth));
        //healthBar.fillAmount = (float)health / (float)maxHealth;
        if (health <= 0) {
            health = maxHealth;
            UpdateHealth();
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence() {
        FPSController thePlayer = transform.root.GetComponent<FPSController>();
        thePlayer.freezeRotation = true;
        thePlayer.freezePosition = true;
        thePlayer.lockMouse = false;
        gameOverOverlay.gameObject.SetActive(true);
        while (gameOverOverlay.color.a <= 1) {
            gameOverOverlay.color = new Color(0, 0, 0, gameOverOverlay.color.a + 0.05f);
            if (Time.timeScale > 0.1f) Time.timeScale -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        Time.timeScale = 0;
        backUpCam.SetActive(true);
        thePlayer.gameObject.SetActive(false);
        SoldierAI[] enemies = FindObjectsOfType<SoldierAI>();
        foreach (SoldierAI enemy in enemies) {
            enemy.gameObject.SetActive(false);
        }
        gameOverUI.SetActive(true);
        StopAllCoroutines();
    }
}
