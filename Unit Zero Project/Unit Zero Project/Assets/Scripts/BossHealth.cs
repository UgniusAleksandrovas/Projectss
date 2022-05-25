using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : EnemyHealth {

    public GameObject winGame;
    public Image gameOverOverlay;
    public GameObject backUpCam;

    public override void UpdateHealth() {
        if (health <= 0) {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence() {
        yield return new WaitForSeconds(3f);
        FPSController thePlayer = FindObjectOfType<FPSController>();
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
        winGame.SetActive(true);
        StopAllCoroutines();
    }
}
